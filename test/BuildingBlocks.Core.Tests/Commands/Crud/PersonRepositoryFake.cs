using BuildingBlocks.EFCore;

namespace BuildingBlocks.Core.Tests.Commands.Crud
{
    public class PersonRepositoryFake : EFCoreRepository<BuildingBlocksCoreDbContext,Person, long>
    {
        public PersonRepositoryFake(BuildingBlocksCoreDbContext context) : base(context)
        {
        }
    }
}