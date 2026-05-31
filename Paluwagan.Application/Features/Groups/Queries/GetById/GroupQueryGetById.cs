using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Messaging.Abstractions;

namespace Paluwagan.Application.Features.Groups.Queries.GetById
{
    public sealed record GroupQueryGetById(Guid groupId) : IQuery<GroupDetailResponse>;
}