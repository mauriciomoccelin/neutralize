using System;
using System.Threading.Tasks;
using BuildingBlocks.Core.UoW;

namespace BuildingBlocks.Core.Tests.Commands
{
    public class UnitOfWork : IUnitOfWork
    {
        public void Dispose() { GC.SuppressFinalize(this); }

        public Task<bool> Commit() { return Task.FromResult(true); }
    }
}