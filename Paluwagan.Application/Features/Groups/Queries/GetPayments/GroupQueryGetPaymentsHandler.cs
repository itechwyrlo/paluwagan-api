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

namespace Paluwagan.Application.Features.Groups.Queries.GetPayments
{
    internal sealed class GroupQueryGetPaymentsHandler(IUnitOfWork unitOfWork, IUserContextService userContext)
        : IQueryHandler<GroupQueryGetPayments, IReadOnlyList<PaymentStatus>>
    {
        public async Task<IReadOnlyList<PaymentStatus>> Handle(GroupQueryGetPayments request, CancellationToken cancellationToken)
        {
            var userId = userContext.GetCurrentUserId();
            var group = await unitOfWork.GroupRepository
                .GetAsync(
                    g => g.Id == new GroupId(request.groupId)
                      && (g.OrganizerId == userId || g.Members.Any(m => m.UserId == userId)),
                    g => g.Members,
                    g => g.Payments)
                .ConfigureAwait(false)
                ?? throw new NotFoundException($"Group {request.groupId} was not found.");

            var memberIds = group.Members.Select(m => m.UserId).ToList();
            var users = await unitOfWork.UserRepository
                .GetAllAsync(u => memberIds.Contains(u.Id))
                .ConfigureAwait(false);

            return group.ToPaymentStatuses(users);
        }
    }
}
