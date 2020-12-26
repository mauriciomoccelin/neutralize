using MediatR;

namespace Neutralize.Commands
{
    public class CommandInt64<TResponse> : Command<long>, IRequest<TResponse>
    {
    }
}