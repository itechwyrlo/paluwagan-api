using Paluwagan.Application.DTOs;
using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.SharedKernel.Models;

namespace Paluwagan.Application.Features.Groups.Queries.GetAll
{
    public sealed record GroupQueryAll(QueryObjectParams? param) : IQuery<PagedResult<GroupSummaryResponse>>;
}