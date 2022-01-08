using System;
using Neutralize.Events;
using FluentValidation.Results;

namespace Neutralize.Commands
{
    public abstract class Command<TId> : Message
    {
        public TId Id { get; set; }
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; protected set; }

        protected Command()
        {
            Timestamp = DateTime.Now; 
            ValidationResult = new ValidationResult();
        }

        public virtual bool Validate() => throw new NotImplementedException();
        public virtual bool IsValid() => ValidationResult.IsValid;
    }
}

