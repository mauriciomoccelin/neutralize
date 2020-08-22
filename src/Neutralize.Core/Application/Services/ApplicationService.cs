using System.Threading.Tasks;
using AutoMapper;
using Neutralize.Bus;
using Neutralize.Notifications;
using Neutralize.UoW;

namespace Neutralize.Application.Services
{
    public abstract class ApplicationService : IApplicationService
    {
        protected IMapper Mapper { get; }
        protected IUnitOfWork UnitOfWork { get; }
        protected IInMemoryBus InMemoryBus { get; }

        protected ApplicationService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IInMemoryBus inMemoryBus
        )
        {
            Mapper = mapper;
            UnitOfWork = unitOfWork;
            InMemoryBus = inMemoryBus;
        }

        protected virtual async Task AddNotificationError(
            string type, string message
        )
        {
            await InMemoryBus.RaiseEvent(DomainNotification.Create(type, message));
        }

        public abstract void Dispose();
    }
}