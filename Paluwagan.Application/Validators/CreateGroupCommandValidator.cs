using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Paluwagan.Application.Features.Groups.Commands;

namespace Paluwagan.Application.Validators
{
    public class CreateGroupCommandValidator : AbstractValidator<CreateGroupCommand>
    {
        public CreateGroupCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Group name is required")
                .MaximumLength(100).WithMessage("Group name cannot exceed 100 characters")
                .Must(n => !n.Contains('<') && !n.Contains('>')
                    && !n.Contains('\0') && !n.Contains('\r') && !n.Contains('\n'))
                    .WithMessage("Group name contains invalid characters");

            RuleFor(x => x.Schedule)
                .IsInEnum().WithMessage("Payment schedule is invalid");

            RuleFor(x => x.ContributionAmount)
                .GreaterThan(0).WithMessage("Contribution amount must be greater than zero")
                .LessThanOrEqualTo(999_999_999.99m).WithMessage("Contribution amount exceeds the maximum allowed value");

            RuleFor(x => x.NumberOfSlots)
                .GreaterThanOrEqualTo(2).WithMessage("A group must have at least 2 slots")
                .LessThanOrEqualTo(500).WithMessage("A group cannot have more than 500 slots");

            RuleFor(x => x.StartDate)
                .Must(d => d.Date >= DateTime.UtcNow.Date).WithMessage("Start date cannot be in the past")
                .Must(d => d.Date <= DateTime.UtcNow.Date.AddYears(10)).WithMessage("Start date cannot be more than 10 years in the future");
        }
    }
}
