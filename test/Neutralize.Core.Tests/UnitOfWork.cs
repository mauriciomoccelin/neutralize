using System.Threading.Tasks;
using Neutralize.UoW;

namespace Neutralize.Tests
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
