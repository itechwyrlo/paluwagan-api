using System;
using System.Collections.Generic;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Messaging.Abstractions;

namespace Paluwagan.Application.Features.Groups.Queries.GetMembers
{
    public sealed record GroupQueryGetMembers(Guid groupId) : IQuery<IReadOnlyList<MemberDetail>>;
}
