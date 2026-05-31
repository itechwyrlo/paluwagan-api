using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Mappings;
using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain;
using Paluwagan.Domain.Services;
using Paluwagan.Domain.ValueObjects;
using Paluwagan.SharedKernel.Exceptions;

namespace Paluwagan.Application.Features.Groups.Queries.GetMembers
{
    internal sealed class GroupQueryGetMembersHandler(IUnitOfWork unitOfWork, IUserContextService userContext)
        : IQueryHandler<GroupQueryGetMembers, IReadOnlyList<MemberDetail>>
    {
        public async Task<IReadOnlyList<MemberDetail>> Handle(GroupQueryGetMembers request, CancellationToken cancellationToken)
        {
            var userId = userContext.GetCurrentUserId();
            var group = await unitOfWork.GroupRepository
                .GetAsync(
                   predicate: g => g.Id == new GroupId(request.groupId)
                                && (g.OrganizerId == userId || g.Members.Any(m => m.UserId == userId)),
                   includes: g => g.Members)
                .ConfigureAwait(false)
                ?? throw new NotFoundException($"Group {request.groupId} was not found.");

            var userIds = group.Members.Select(m => m.UserId).ToList();
            var users = await unitOfWork.UserRepository
                .GetAllAsync(u => userIds.Contains(u.Id))
                .ConfigureAwait(false);
            

            return group.ToMemberDetails(users);
        }
    }
}
