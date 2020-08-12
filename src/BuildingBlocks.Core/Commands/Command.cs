using System;
using BuildingBlocks.Core.Events;
using FluentValidation.Results;

namespace BuildingBlocks.Core.Commands
{
    public abstract class Command : Message
    {
        public long Id { get; protected set; }
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
