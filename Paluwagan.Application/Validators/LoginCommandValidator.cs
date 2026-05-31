using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Paluwagan.Application.Features.Login.Commands;

namespace Paluwagan.Application.Validators
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email must be a valid email address")
                .MaximumLength(256).WithMessage("Email cannot exceed 256 characters")
                .Must(e => !e.Contains("javascript:", StringComparison.OrdinalIgnoreCase)
                    && !e.Contains('\0')
                    && !e.Contains('\r')
                    && !e.Contains('\n'))
                    .WithMessage("Email contains invalid characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .MaximumLength(256).WithMessage("Password cannot exceed 256 characters")
                .Must(p => !p.Contains('\0')).WithMessage("Password contains invalid characters");
        }
    }
}