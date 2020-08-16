using System;
using BuildingBlocks.Events;
using FluentValidation.Results;

namespace BuildingBlocks.Commands
{
    public abstract class Command : Command<Guid>
    {
    }
    
    public abstract class Command<TId> : Message
    {
        public TId Id { get; protected set; }
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; protected set; }

        protected Command()
        {
            Timestamp = DateTime.Now; 
            ValidationResult = new ValidationResult();
        }
        
        public abstract bool Validate();
        public virtual bool IsValid() => ValidationResult.IsValid;
    }
}
