using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Neutralize.UoW;

namespace Neutralize.EFCore
{
    public sealed class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
    {
        private readonly TDbContext dbContext;
        private readonly ILogger<UnitOfWork<TDbContext>> logger;
        
        public UnitOfWork(ILogger<UnitOfWork<TDbContext>> logger, TDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }
        
        public void Dispose()
        {
            dbContext.Dispose();
        }

        public async Task<bool> Commit()
        {
            try
            {
                var success = await dbContext.SaveChangesAsync() > 0;
                return success;
            }
            catch(Exception exception)
            {
                logger.LogError(exception, "UnitOfWork:Commit");
                return false;
            }
        }
    }
}