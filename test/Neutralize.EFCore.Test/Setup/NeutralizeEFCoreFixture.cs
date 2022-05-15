using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Neutralize.UoW;
using Xunit;

namespace Neutralize.EFCore.Test.Setup
{
    [CollectionDefinition(nameof(NeutralizeEFCoreCollection))]
    public class NeutralizeEFCoreCollection : ICollectionFixture<NeutralizeEFCoreFixture> {}
    
    public class NeutralizeEFCoreFixture : IDisposable
    {
        public IUnitOfWork UnitOfWork { get; set; }
        public TodoDbContext TodoDbContext { get; set; }
        public TodoRepository TodoRepository { get; set; }

        public void Dispose()
        {
            UnitOfWork?.Dispose();
            TodoDbContext?.Dispose();
            TodoRepository?.Dispose();
        }
        
        public TodoDbContext GeneretDbCOntext()
        {
            var options = new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase("Testing")
                .Options;

            return new TodoDbContext(options);
        }

        public IUnitOfWork GenereteUnitOfWork(TodoDbContext dbContext = null)
        {
            var logger = new Logger<UnitOfWork<TodoDbContext>>(new LoggerFactory());
            
            TodoDbContext = dbContext ?? GeneretDbCOntext();
            UnitOfWork = new UnitOfWork<TodoDbContext>(logger, dbContext);

            return UnitOfWork;
        }

        public TodoRepository GenereteSQLiteRepository(TodoDbContext dbContext = null)
        {
            var options = new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase("Testing")
                .Options;
            
            TodoDbContext = dbContext ?? GeneretDbCOntext();
            TodoRepository = new TodoRepository(TodoDbContext);
            
            return TodoRepository;
        }

        public async Task<IEnumerable<ToDo>> SeedForTodo(TodoRepository repository, int count = 10)
        {
            var todos = GenereteTodos(count);

            foreach (var todo in todos)
                await repository.AddAsync(todo);

            await repository.Commit();

            return todos;
        }

        public IEnumerable<ToDo> GenereteTodos(int count)
        {
            var faker = new Faker<ToDo>()
                .CustomInstantiator(fake => new ToDo
                {
                    Done = fake.Random.Bool(),
                    Desacription = fake.Lorem.Text()
                });

            return faker.Generate(count);
        }
        
        public ToDo GenereteTodo()
        {
            return GenereteTodos(1).First();
        }
    }
}