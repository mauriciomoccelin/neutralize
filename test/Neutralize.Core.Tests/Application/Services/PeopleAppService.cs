using AutoMapper;
using Neutralize.Application.Services;
using Neutralize.Bus;
using Neutralize.Repositories;
using Neutralize.Tests.Models;
using Neutralize.UoW;

namespace Neutralize.Tests.Application.Services
{
    public class PeopleAppService : CrudAppService<People, PeopleDto>, IPeopleAppService
    {
        public PeopleAppService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IInMemoryBus inMemoryBus,
            IRepository<People, long> repository
        ) : base(mapper, unitOfWork, inMemoryBus, repository)
        {
        }
    }
}