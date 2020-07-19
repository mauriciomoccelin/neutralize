using System;
using System.Threading.Tasks;

namespace BuildingBlocks.Core.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Commit();
    }
}
