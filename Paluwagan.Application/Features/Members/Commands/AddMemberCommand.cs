using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paluwagan.Application.Messaging.Abstractions;

namespace Paluwagan.Application.Features.Members.Commands
{
    public record AddMemberCommand(Guid GroupId, string AccountId, int SlotNumber) : ICommand;
}
