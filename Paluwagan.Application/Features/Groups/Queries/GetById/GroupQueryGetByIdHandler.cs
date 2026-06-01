using Paluwagan.Application.DTOs;
using Paluwagan.Application.Mappings;
using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain;
using Paluwagan.Domain.Services;
using Paluwagan.Domain.ValueObjects;
using Paluwagan.SharedKernel.Exceptions;

namespace Paluwagan.Application.Features.Groups.Queries.GetById
{
    internal sealed class GroupQueryGetByIdHandler(IUnitOfWork unitOfWork, IUserContextService userContext) : IQueryHandler<GroupQueryGetById, GroupDetailResponse>
    {
        public async Task<GroupDetailResponse> Handle(GroupQueryGetById request, CancellationToken cancellationToken)
        {
            var userId = userContext.GetCurrentUserId();
            var group = await unitOfWork.GroupRepository
                .GetAsync(
                    predicate: g => g.Id == new GroupId(request.groupId)
                                 && (g.OrganizerId == userId || g.Members.Any(m => m.UserId == userId)),
                    includes: g => g.Payments)
                .ConfigureAwait(false)
                ?? throw new NotFoundException($"Group {request.groupId} was not found.");

            var organizer = await unitOfWork.UserRepository
                .GetByIdAsync(group.OrganizerId)
                .ConfigureAwait(false)
                ?? throw new NotFoundException($"Organizer for group {request.groupId} was not found.");

            return group.ToGroupDetail(organizer);
        }
    }
}