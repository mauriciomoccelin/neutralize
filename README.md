# Neutralize

## A .NET Core Web Application Framework

### About Core

In the namespace `Neutralize.Models` exists the `Entity<TId>`. This class can be used to represent a entity of database, example:

```csharp
public class ToDo : Entity<Guid>
{
    public bool Done { get; set; }
    public string Desacription { get; set; }
}
```

For a single unit entity, on domain, can be used `AggregateRoot<TId>`. This class has a list of events that can be adding to be triged for example when you commit with success operations over data base.

> `TId` must me a struct type.

### About Repository

The interface `IRepository<TEntity, in TId>` expose the default operation over a entity maped in the application domain.

```csharp
public interface IRepository<TEntity, in TId> : IDisposable
    where TId : struct
    where TEntity : IEntity<TId>
{
    Task<bool> Commit();
    Task AddAsync(TEntity entity);
    Task RemoveAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task<TEntity> GetAsync(TId id);
    IQueryable<TEntity> GetAll();
    Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> FirstOrAsync(Expression<Func<TEntity, bool>> predicate, TEntity @default = default(TEntity));
}
```

You can use IoC to specify the implementation of this interface or use the default implementation with EFCore as ORM.

1. Fist create a DBContext

```csharp
public sealed class NeutralizeDBContext : DbContext
{
    // DbSet<TEntity> here

    public DbSet<ToDo> ToDos { get; set; }

    public NeutralizeDBContext(
        DbContextOptions<NeutralizeDBContext> options
    ) : base(options)
    {
        ChangeTracker.AutoDetectChangesEnabled = false;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Ignore<Event>();
        modelBuilder.Ignore<ValidationResult>();
        modelBuilder.SetDefaultColumnTypeVarchar();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NeutralizeDBContext).Assembly);
    }
}
```

2. Create a class to represent the repository implementation.

```csharp
public class Repository<TEntity, TId> : EfCoreRepository<NeutralizeDBContext, TEntity, TId>
    where TEntity : Entity<TId>
    where TId : struct
{
    public Repository(NeutralizeDBContext context) : base(context)
    {
    }
}
```

#### IUnitOfWork

If you use `CommandHandler` in the application you must me implementing and register `IUnitOfWork` interface. Example:

```csharp
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly NeutralizeTemplateDBContext dbContext;

    public UnitOfWork(NeutralizeTemplateDBContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void Dispose()
    {
        dbContext.Dispose();
    }

    public async Task<bool> Commit()
    {
        return await dbContext.SaveChangesAsync() > 0;
    }
}
```

3. Then register the implementation for `IRepository<TEntity, in TId>`

```csharp
public static void AddDatabaseConfiguration(this IServiceCollection services)
{
    if (services == null) throw new ArgumentNullException(nameof(services));

    services.AddScoped<IUnitOfWork,  UnitOfWork>();

    services.AddDbContext<NeutralizeTemplateDBContext>(
        options => options.UseSqlServer("DefaultConnection")
    );

    services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
}
```

### Dapper

If needs Dapper to read data on database with custom query that EF Core, for any reason, can't read use de interface `Dapper Repository`.

```csharp
public interface IDapperRepository : IDisposable
{
    void AddParameter(string name, object value);
    Task<Option<T>> First<T>(string command);
    Task<Option<PagedResultDto<T>>> Paged<T>(string command);
}
```

This class has a concrete default implementation with class `Dapper Repository`. This class receive a interface `Dapper ConnectionFactory` to create opened database connection as you well.

```csharp
public interface IDapperConnectionFactory
{
    IDbConnection CreateOpened();
}
```

> Paged execute a `QueryMultipleAsync` of Dapper then is required split the query with `;`. The first query is total count and the second query is to get itens.

```csharp
repository.AddParameter("PageSize", 10);
repository.AddParameter("PageNumber", 1);

const string commandCount = "SELECT COUNT(Id) FROM ToDo";
const string commandSelectFromTodo = @"
    SELECT * FROM ToDo
    ORDER BY ToDo.Id
    OFFSET @PageSize * (@PageNumber - 1) ROWS
    FETCH NEXT @PageSize ROWS ONLY";
const string command = commandCount + ";" + commandSelectFromTodo;

var queryPaged = await dapperRepository.Paged<ToDoList>(command);
```

## Applications

If you has a event-driven application is possible use the `IInMemoryBus` to send command and handler.

> `IInMemoryBus` use the MediatR package as base.

1. Create you command and validation.

- Command

```csharp
public class MarkToDoAsDoneCommand : CommandGuid<Guid>
{
    public MarkToDoAsDoneCommand() { }
    public MarkToDoAsDoneCommand(Guid id)
    {
        Id = id;
    }

    public override bool Validate()
    {
        ValidationResult = new MarkToDoAsDoneValidator().Validate(this);
        return ValidationResult.IsValid;
    }
}
```

- Validation

```csharp
using FluentValidation;

namespace Neutralize.Template.ToDos.Commands
{
    public class MarkToDoAsDoneValidator : AbstractValidator<MarkToDoAsDoneCommand>
    {
        public MarkToDoAsDoneValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id cannot be less than zero");
        }
    }
}
```

2. Create a Handler for this command.

```csharp
namespace Neutralize.Application;

public sealed class ToDoCommandHandler :
    CommandHandler,
    IRequestHandler<MarkToDoAsDoneCommand, Guid>
{
    private readonly IInMemoryBus inMemoryBus;
    private readonly IRepository<ToDo, Guid> todoRepository;

    public ToDoCommandHandler(
        IUnitOfWork unitOfWork,
        IInMemoryBus inMemoryBus,
        IRepository<ToDo, Guid> todoRepository,
        INotificationHandler<DomainNotification> notifications
    ) : base(unitOfWork, inMemoryBus, notifications)
    {
        this.inMemoryBus = inMemoryBus;
        this.todoRepository = todoRepository;
    }

    public override void Dispose()
    {
        todoRepository.Dispose();
    }

    public async Task<Guid> Handle(MarkToDoAsDoneCommand request, CancellationToken cancellationToken)
    {
        await CheckErrors(request);
        if (!request.IsValid()) return default;

        var todo = await todoRepository.GetAsync(request.Id);

        if (todo is null)
        {
            await inMemoryBus.RaiseEvent(
                DomainNotification.Create("MarkToDoAsDone", $"ToDo not found {request.Id}")
            );

            return Guid.Empty;
        }

        todo.MarkAsDone();

        await todoRepository.UpdateAsync(todo);
        await Commit();

        return todo.Id;
    }
}
```

3. Register the dependencies

```csharp
public static void AddDatabaseConfiguration(this IServiceCollection services)
{
    var assembly = Assembly.Load("Neutralize.Application");

    services.AddMediatR(assembly);
    services.AddScoped<IInMemoryBus, InMemoryBus>();
    services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
}
```

4. Receiving request then publish a command

In your API, for example, you can receive the request as a command and send to be handled.

```csharp
[Authorize]
[ApiController]
[Route("todos")]
public class ToDoController : Controller
{
    private readonly IInMemoryBus inMemoryBus;

    public ToDoController(IInMemoryBus inMemoryBus)
    {
        this.inMemoryBus = inMemoryBus;
    }

    [HttpPatch]
    [Route("{id:guid}/mark-as-done")]
    [ProducesResponseType(typeof(Guid), 200)]
    public async Task<IActionResult> MarkAsDone([FromRoute]Guid id)
    {
        var command = new MarkToDoAsDoneCommand(id);
        var result = await inMemoryBus.SendCommandGuidId<Guid>(command);

        // custon response
        return ResponseWrapper(result);
    }
}
```

---

### Kafka and MediatR

Integration between [MediatR](https://github.com/jbogard/MediatR) and [Confluent.Kafka](https://github.com/confluentinc/confluent-kafka-dotnet) library.

1° Register the event types for the topic and the assembly where the handlers are.

```csharp
using Neutralize.Kafka;

public void ConfigureServices(IServiceCollection services)
{
    services.AddKafka(
        options => {
            const string group = "*";
            const string topic = "forecast";
            const string server = "host.kafka.com";

            options.EnableMonitor(true);
            options.SetFlushTimeout(10);
            options.SetProducerConfig(server);
            options.SetConsumerConfig(group, server);
            options.AddHandler(forecast, typeof(WeatherForecast));
        }
    );
}
```

- `forecast`: It is the topic to subscribe.
- `WeatherForecast`: The topic message type

> The entire message posted on the `forecast` topic will be read to the` WeatherForecast`. Now we just need to configure the handler for the messages. This is where MediatR comes in.

If you just need a producer implementation, do this:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddKafka(
        options => {
            const string server = "host.kafka.com";

            options.SetFlushTimeout(10);
            options.EnableMonitor(false);
            options.SetProducerConfig(server);
        }
    );
}
```

#### **Notification**

```csharp
using MediatR;

public class WeatherForecast : INotification
{
    public DateTime Date { get; set; }
    public string Summary { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);

    public override string ToString() => $"{Date} | {Summary} | C°: {TemperatureC} | F°: {TemperatureF}";
}
```

> `WeatherForecast` to implement the `INotification` markup interface so that MediatR knows who to delegate this message to.

#### Notification Handler

```csharp
using MediatR;

public class WeatherForecastHandler : INotificationHandler<WeatherForecast>
{
    public Task Handle(WeatherForecast notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(notification);
        return Task.CompletedTask;
    }
}
```

#### **Producer**

Just inject the `IKafkaFactory` interface into the routine where you want to send notifications on for a specific topic.

```csharp
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Neutralize.Kafka;

namespace kafka.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly IProducer<string, WeatherForecast> producer;

    public WeatherForecastController(IKafkaFactory kafkaFactory)
    {
        producer = kafkaFactory.CreateProducer<string, WeatherForecast>();
    }

    [HttPost(Name = "GetWeatherForecast")]
    public WeatherForecast Produce()
    {
        var weatherForecast = new WeatherForecast
        {
            Date = DateTime.Now.AddDays(Random.Shared.Next(1, 8)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        };

        var message = new Message<string, WeatherForecast>()
        {
            Key = Guid.NewGuid().ToString(),
            Value = weatherForecast
        };

        producer.Produce("forecast", message);

        return weatherForecast;
    }
}

```

After invoking the api with the POST verb the output will be as follows:

```bash
Received message: tópic (forecast), offset (126), partition (0)
01/01/2021 9:41:56 PM | Sweltering | C°: 16 | F°: 60
```

#### **Consumer**

The monitor consumer who reads the messages of the topic and delegates to the attendant with MediatR runs as a task in second plan as an `IHostedService`. Look [Background Tasks With IHostedService](https://docs.microsoft.com/pt-br/dotnet/architecture/microservices/multi-container-microservice-net-applications/background-tasks-with-ihostedservice)
