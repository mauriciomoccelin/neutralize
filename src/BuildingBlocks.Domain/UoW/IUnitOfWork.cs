using System;
using System.Threading.Tasks;

namespace BuildingBlocks.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Commit();
    }
}
