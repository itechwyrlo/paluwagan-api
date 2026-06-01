using System;
using Paluwagan.Application.Messaging.Abstractions;

namespace Paluwagan.Application.Features.Notifications.Queries.GetNotificationStatus
{
    public sealed record GetNotificationStatusQuery(Guid? TargetUserId = null)
        : IQuery<NotificationStatusResponse>;
}
