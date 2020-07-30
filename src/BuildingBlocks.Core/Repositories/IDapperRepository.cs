using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using BuildingBlocks.Core.Models;

namespace BuildingBlocks.Core.Repositories
{
    public interface IDapperRepository<TEntity, in TId> : IRepository<TEntity, TId> 
        where TEntity : Entity<TEntity, TId> 
        where TId : struct
    {
        DbConnection Connection { get; }
        Task<IEnumerable<TEntity>> GetAll();
    }
}
