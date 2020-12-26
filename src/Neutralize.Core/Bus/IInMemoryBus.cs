using MediatR;

namespace Neutralize.Bus
{
    public interface IInMemoryBus : INeutralizeBus
    {
        IMediator Mediator { get; }
    }
}