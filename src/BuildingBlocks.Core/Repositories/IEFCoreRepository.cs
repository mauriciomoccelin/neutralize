using System.Threading.Tasks;

namespace BuildingBlocks.Core.Repositories
{
    /// <summary>
    /// Base interface for custom repositories of the EF Core.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TId">Primary key type of the entity</typeparam>
    public interface IEFCoreRepository<TEntity, in TId> 
        : IRepository<TEntity, TId> where TEntity: class where TId: struct
    {
        Task<int> SaveChangesAsync();
    }
}