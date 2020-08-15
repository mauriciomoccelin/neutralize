using System.Threading.Tasks;
using BuildingBlocks.Core.UoW;

namespace BuildingBlocks.Core.Tests
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        public void Dispose() { }

        public Task<bool> Commit()
        {
            return Task.FromResult(true);
        }
    }
}