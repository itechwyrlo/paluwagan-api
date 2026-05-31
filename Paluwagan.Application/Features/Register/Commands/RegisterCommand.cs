using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Messaging.Abstractions;

namespace Paluwagan.Application.Features.Register.Commands
{
    public record RegisterCommand(
        string FullName,
        string Email,
        string Password,
        string ConfirmPassword,
        string Role
    ) : ICommand<RegisterResponse>;
}
