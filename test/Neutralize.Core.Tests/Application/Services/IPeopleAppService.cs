using Neutralize.Application.Services;
using Neutralize.Tests.Models;

namespace Neutralize.Tests.Application.Services
{
    public interface IPeopleAppService : ICrudAppService<long, PeopleDto>
    {
    }
}