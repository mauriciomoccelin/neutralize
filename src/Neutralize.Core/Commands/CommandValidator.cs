using System;
using FluentValidation;

namespace Neutralize.Commands
{
    public abstract class CommandValidator<T> : AbstractValidator<T>
    {
    }

    public abstract class CommandValidator<TCommand, TId> : CommandValidator<TCommand> where TCommand : Command<TId> 
    {
    }
    
    public abstract class CommandInt64Validator<TCommand> : CommandValidator<TCommand> where TCommand : Command<long> 
    {
    }
    
    public abstract class CommandGuidValidator<TCommand> : CommandValidator<TCommand> where TCommand : Command<Guid> 
    {
    }
}