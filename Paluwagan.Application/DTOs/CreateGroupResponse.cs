using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paluwagan.Domain.ValueObjects;

namespace Paluwagan.Application.DTOs
{
    public sealed record CreateGroupResponse
    {
        public GroupId? GroupId { get; init; }
    }
}
