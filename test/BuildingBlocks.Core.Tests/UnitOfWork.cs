using System.Threading.Tasks;
using BuildingBlocks.UoW;

namespace BuildingBlocks.Tests
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