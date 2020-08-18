using AutoMapper;
using Neutralize.UoW;

namespace Neutralize.Application.Services
{
    public abstract class ApplicationService : IApplicationService
    {
        protected IMapper Mapper { get; }
        protected IUnitOfWork UnitOfWork { get; }
        
        protected ApplicationService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            Mapper = mapper;
            UnitOfWork = unitOfWork;
        }

        public abstract void Dispose();
    }
}