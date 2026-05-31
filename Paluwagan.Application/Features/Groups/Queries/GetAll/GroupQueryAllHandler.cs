using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Mappings;
using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain;
using Paluwagan.Domain.Services;
using Paluwagan.SharedKernel.Models;

namespace Paluwagan.Application.Features.Groups.Queries.GetAll
{
    internal sealed class GroupQueryAllHandler : IQueryHandler<GroupQueryAll, PagedResult<GroupSummaryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContext;

        public GroupQueryAllHandler(IUnitOfWork unitOfWork, IUserContextService userContext)
        {
            _unitOfWork = unitOfWork;
            _userContext = userContext;
        }

        public async Task<PagedResult<GroupSummaryResponse>> Handle(
            GroupQueryAll query,
            CancellationToken cancellationToken)
        {
            var userId = _userContext.GetCurrentUserId();
            var result = await _unitOfWork.GroupRepository.GetPageAsync(
                query.param,
                predicate: g => g.OrganizerId == userId || g.Members.Any(m => m.UserId == userId),
                includes: x => x.Payments)
                .ConfigureAwait(false);

            var groupSummaries = result.data.Select(g => g.ToGroupSummary(userId)).ToList();
            return new PagedResult<GroupSummaryResponse>(groupSummaries, result.total);
        }
    }
}
