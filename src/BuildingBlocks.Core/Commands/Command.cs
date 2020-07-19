using System;
using BuildingBlocks.Core.Events;
using FluentValidation.Results;

namespace BuildingBlocks.Core.Commands
{
    public abstract class Command : Message
    {
        public Guid Id { get; protected set; }
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; protected set; }

        protected Command() { Timestamp = DateTime.Now; }
        
        public abstract bool Validate();
        public virtual bool IsValid() => ValidationResult.IsValid;
    }
}
