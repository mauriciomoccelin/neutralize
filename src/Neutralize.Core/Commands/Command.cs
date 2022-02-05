using System;
using System.Collections.Generic;
using Neutralize.Events;
using FluentValidation.Results;
using Neutralize.Application;

namespace Neutralize.Commands
{
    public abstract class Command<TId> : Message, IShouldNormalizer
    {
        public TId Id { get; set; }
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; protected set; }

        protected Command()
        {
            Timestamp = DateTime.Now; 
            ValidationResult = new ValidationResult();
        }

        public virtual void Normalize() { }
        public virtual bool IsValid() => ValidationResult.IsValid;
        public virtual bool Validate() => throw new NotImplementedException();
        public virtual IEnumerable<ValidationFailure> GetErrors() => ValidationResult.Errors;
    }
}

