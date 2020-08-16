using System;
using System.Threading.Tasks;

namespace BuildingBlocks.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Commit();
    }
}
