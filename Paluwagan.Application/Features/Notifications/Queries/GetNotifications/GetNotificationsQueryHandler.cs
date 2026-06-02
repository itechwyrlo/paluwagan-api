using System.Collections.Generic;
using System.Linq;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Mappings;
using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain;
using Paluwagan.Domain.Services;

namespace Paluwagan.Application.Features.Notifications.Queries.GetNotifications
{
    internal sealed class GetNotificationsQueryHandler(
        IUnitOfWork unitOfWork,
        IUserContextService userContext)
        : IQueryHandler<GetNotificationsQuery, IReadOnlyList<NotificationResponse>>
    {
        public async Task<IReadOnlyList<NotificationResponse>> Handle(GetNotificationsQuery query, CancellationToken cancellationToken)
        {
            var userId = userContext.GetCurrentUserId();

            var notifications = await unitOfWork.NotificationRepository
                .GetByUserIdAsync(userId, cancellationToken)
                .ConfigureAwait(false);

            return notifications
                .Select(n => n.ToNotificationResponse())
                .ToList();
        }
    }
}
