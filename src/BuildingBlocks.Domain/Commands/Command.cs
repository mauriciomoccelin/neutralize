using FluentValidation.Results;
using System;

namespace BuildingBlocks.Domain
{
    public abstract class Command : Message
    {
        public Guid Id { get; protected set; }
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; set; }

        protected Command() { Timestamp = DateTime.Now; }

        public abstract bool IsValid();
    }
}
