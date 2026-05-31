using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Paluwagan.Application.Features.Members.Commands;

namespace Paluwagan.Application.Validators
{
    public class AddMemberCommandValidator : AbstractValidator<AddMemberCommand>
    {
        public AddMemberCommandValidator()
        {
            RuleFor(x => x.GroupId)
                .NotEmpty().WithMessage("Group ID is required");

            RuleFor(x => x.AccountId)
                .NotEmpty().WithMessage("Account ID is required")
                .Matches(@"^PAL-[A-F0-9]{8}$").WithMessage("Account ID format is invalid.");

            RuleFor(x => x.SlotNumber)
                .GreaterThan(0).WithMessage("Slot number must be greater than 0")
                .LessThanOrEqualTo(500).WithMessage("Slot number exceeds the maximum allowed value");
        }
    }
}
