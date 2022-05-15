using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Neutralize.UoW;

namespace Neutralize.EFCore.Test.Setup
{
    public sealed class TodoDbContext : DbContext, IUnitOfWork
    {
        public DbSet<ToDo> ToDos { get; set; }
        
        public TodoDbContext(
            DbContextOptions<TodoDbContext> options
        ) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SetDefaultColumnTypeVarchar();
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("Neutralize.EFCore.Test"));

            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> Commit()
        {
            return await SaveChangesAsync() > 0;
        }
    }
}