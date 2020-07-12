using System;
using System.Threading.Tasks;

namespace BuildingBlocks.Domain.Tests.Commands
{
    public class UnitOfWork : IUnitOfWork
    {
        public void Dispose() { GC.SuppressFinalize(this); }

        public Task<bool> Commit() { return Task.FromResult(true); }
    }
}