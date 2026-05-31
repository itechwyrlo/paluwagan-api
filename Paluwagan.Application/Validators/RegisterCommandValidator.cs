using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Paluwagan.Application.Features.Register.Commands;

namespace Paluwagan.Application.Validators
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        private static readonly string[] AllowedRoles = ["Organizer", "Member"];

        public RegisterCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters")
                .Must(n => !n.Contains('<') && !n.Contains('>')
                    && !n.Contains('\0') && !n.Contains('\r') && !n.Contains('\n'))
                    .WithMessage("Full name contains invalid characters");

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
                .Must(p => !p.Contains('\0')).WithMessage("Password contains invalid characters")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one digit")
                .Matches(@"[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required")
                .Equal(x => x.Password).WithMessage("Passwords do not match");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required")
                .Must(r => AllowedRoles.Contains(r, StringComparer.OrdinalIgnoreCase))
                    .WithMessage("Role must be 'Organizer' or 'Member'");
        }
    }
}
