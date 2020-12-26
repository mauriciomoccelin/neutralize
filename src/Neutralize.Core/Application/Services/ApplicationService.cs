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
        protected INeutralizeBus NeutralizeBus { get; }

        protected ApplicationService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            INeutralizeBus neutralizeBus
        )
        {
            Mapper = mapper;
            UnitOfWork = unitOfWork;
            NeutralizeBus = neutralizeBus;
        }

        protected virtual async Task AddNotificationError(
            string type, string message
        )
        {
            await NeutralizeBus.RaiseEvent(DomainNotification.Create(type, message));
        }

        public abstract void Dispose();
    }
}