using AutoMapper;
using BuildingBlocks.Core.Application;
using BuildingBlocks.Core.Bus;
using BuildingBlocks.Core.Commands;
using BuildingBlocks.Core.Notifications;
using BuildingBlocks.Core.Repositories;
using BuildingBlocks.Core.UoW;
using MediatR;

namespace BuildingBlocks.Core.Tests.Commands.Crud
{
    public class PersonCommandHandler :
        CrudCommandHandler<
            Person,
            PersonDto,
            long,
            CreatePersonCommand,
            UpdatePersonCommand,
            DeletePersonCommand,
            GetPersonCommand,
            GetPagedPersonCommand
        >,
        IRequestHandler<CreatePersonCommand>,
        IRequestHandler<UpdatePersonCommand>,
        IRequestHandler<DeletePersonCommand>,
        IRequestHandler<GetPersonCommand, PersonDto>,
        IRequestHandler<GetPagedPersonCommand, PagedResultDto<PersonDto>>
    {
        public PersonCommandHandler(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IInMemoryBus inMemoryBus,
            IEFCoreRepository<Person, long> repository,
            INotificationHandler<DomainNotification> notifications
        ) : base(mapper, unitOfWork, inMemoryBus, repository, notifications)
        {
        }

        public override void Dispose()
        {
            Repository.Dispose();
        }
    }
}