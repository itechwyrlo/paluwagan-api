using System.Collections.Generic;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Messaging.Abstractions;

namespace Paluwagan.Application.Features.Notifications.Queries.GetNotifications
{
    public sealed record GetNotificationsQuery() : IQuery<IReadOnlyList<NotificationResponse>>;
}
