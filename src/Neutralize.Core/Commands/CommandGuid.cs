using System;
using MediatR;

namespace Neutralize.Commands
{
    public class CommandGuid<TResponse> : Command<Guid>, IRequest<TResponse>
    {
    }
}