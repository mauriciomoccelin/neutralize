﻿using BuildingBlocks.Core.Commands;
using MediatR;
using Optional;

namespace BuildingBlocks.Core.Tests.Commands
{
    public class AddProductCommand : Command, IRequest<Option<string>>
    {
        public string Name { get; private set; }

        public AddProductCommand(string name) { Name = name; }

        public override bool Validate()
        {
            ValidationResult = new AddProductValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}