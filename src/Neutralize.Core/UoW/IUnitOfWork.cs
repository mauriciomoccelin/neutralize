using System;
using System.Threading.Tasks;

namespace Neutralize.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Commit();
    }
}

