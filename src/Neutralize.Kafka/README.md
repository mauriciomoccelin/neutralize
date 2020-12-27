# Kafka and MediatR

Integration between [MediatR](https://github.com/jbogard/MediatR) and [Confluent.Kafka](https://github.com/confluentinc/confluent-kafka-dotnet) library.

## Setup

1° Add the settings for connection to the Kafka cluster in your `appsettings.json` or ` appsettings.{Env.EnvironmentName}.json`.

```
"kafka": {
  "group": "*",
  "flushTimeout": 10,
  "bootstrapServers": "localhost:9092",
  "topicFailureDelivery": "logs.delivery.failed",
  "topicSuccessDelivery": "logs.delivery.success"
}
```

2 ° Register the event types for the topic and the assembly where the handlers are.

```
using Neutralize.Kafka;
...

public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddKafka(
        Configuration,
        options => options.AddHandler("forecast", typeof(WeatherForecast)),
        Assembly.Load("Neutralize.Mock.WebApi")
    );
}
```

- `forecast`: It is the topic to subscribe.
- `WeatherForecast`: The topic message type

The entire message posted on the `forecast` topic will be read to the` WeatherForecast`. Now we just need to configure the handler for the messages. This is where MediatR comes in.

- > To register only the message producer module.

```
using Neutralize.Kafka;
...

public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddKafka(Configuration);
}
```

### Notification
```
using MediatR;
...

public class WeatherForecast : INotification
{
    public DateTime Date { get; set; }
    public string Summary { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);

    public override string ToString() => $"{Date} | {Summary} | C°: {TemperatureC} | F°: {TemperatureF}";
}
```

`WeatherForecast` to implement the `INotification` markup interface so that MediatR knows who to delegate this message to.


### Notification Handler

```
...
using MediatR;
...

public class WeatherForecastHandler : INotificationHandler<WeatherForecast>
{
    public Task Handle(WeatherForecast notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(notification);
        return Task.CompletedTask;
    }
}
```

## Producer

Just inject the `IKafkaProducer` interface into the routine where you want to send notifications on for a specific topic.

- > Dependency injection considers `Microsoft.Extensions.DependencyInjection.Abstractions`

```
using Neutralize.Kafka.Productors;
...

private readonly IKafkaProducer _producer;

public WeatherForecastController(IKafkaProducer producer)
{
    _producer = producer;
}

[HttpPost]
public WeatherForecast Post([FromBody] WeatherForecast forecast)
{
    _producer?.ProduceAsync("forecast", forecast);
    return forecast;
}

```

After invoking the api with the POST verb the output will be as follows

```
Received message: tópic (forecast), offset (126), partition (0)
01/01/2021 9:41:56 PM | Sweltering | C°: 16 | F°: 60
```

### Report Delivery
- > When `logs.delivery.failed` is informed if the message has failed to send to the cluster topic, an attempt will be made to save the message to the delivery failure topic.

- > When `logs.delivery.success` is informed if the message is successfully sent to the topic in the cluster, an attempt will be made to save the message to the delivery successes topic.

## Consumer

The monitor consumer who reads the messages of the topic and delegates to the attendant with MediatR runs as a task in second plan as an `IHostedService`. Look [Background Tasks With IHostedService](https://docs.microsoft.com/pt-br/dotnet/architecture/microservices/multi-container-microservice-net-applications/background-tasks-with-ihostedservice)