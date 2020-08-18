using System.Threading.Tasks;
using FluentAssertions;
using Neutralize.Application;
using Neutralize.Tests.Models;
using Xunit;

namespace Neutralize.Tests.Application.Services
{
    public class CrudAppService_Test : NeutralizeCoreBaseTest
    {
        private readonly IPeopleAppService appService;

        public CrudAppService_Test()
        {
            appService = Resolve<IPeopleAppService>();
        }

        [Fact]
        public async Task CrudService_Test()
        {
            // Mock
            var dto = new PeopleDto() {};
            
            // Act
            var id = await appService.Create(dto);
            dto.Id = id;
            id = await appService.Update(dto);

            dto = await appService.Get(new EntityDto<long>() {Id = id});
            var list = await appService.GetList(new PagedRequestDto()
            {
                Page = 0,
                PageSize = 10
            });
            
            // Assert

            dto.Should().NotBeNull();
            id.Should().BeGreaterThan(0);
            list.TotalCount.Should().Be(0);
        }

        public override void Dispose()
        {
            appService.Dispose();
        }
    }
}