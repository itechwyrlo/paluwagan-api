using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paluwagan.Application.Messaging.Abstractions;

namespace Paluwagan.Application.Features.Logout.Commands
{
   public sealed record LogoutCommand() : ICommand;
}