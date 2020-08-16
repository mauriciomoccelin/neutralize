using System.Threading.Tasks;
using FluentValidation.Results;
using Neutralize.UoW;
using Microsoft.EntityFrameworkCore;
using Neutralize.Bus;
using Neutralize.Events;
using Neutralize.Extensions;
using Neutralize.Tests.Models;

namespace Neutralize.Tests
{
    public class NeutralizeCoreDbContext : DbContext, IUnitOfWork
    {
        private readonly IInMemoryBus bus;
        
        public DbSet<People> Peoples { get; set; } 
        public DbSet<Product> Products { get; set; } 
        
        public NeutralizeCoreDbContext(
            IInMemoryBus bus,
            DbContextOptions<NeutralizeCoreDbContext> options
        ) : base(options)
        {
            this.bus = bus;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Entity<People>().OwnsOne(x => x.Address);
            
            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> Commit()
        {
            await bus.PublishDomainEvents(this).ConfigureAwait(false);
            return await base.SaveChangesAsync() > 0;
        }
    }
}
