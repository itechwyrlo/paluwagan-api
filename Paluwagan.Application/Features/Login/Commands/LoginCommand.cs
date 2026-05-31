using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Messaging.Abstractions;

namespace Paluwagan.Application.Features.Login.Commands
{
    public record LoginCommand(string Email, string Password) : ICommand<AuthResponse>;
}