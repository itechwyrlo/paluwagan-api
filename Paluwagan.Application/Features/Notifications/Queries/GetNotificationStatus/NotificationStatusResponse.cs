using System;

namespace Paluwagan.Application.Features.Notifications.Queries.GetNotificationStatus
{
    public sealed record NotificationStatusResponse(
        Guid UserId,
        bool HasFcmToken,
        string? TokenPreview
    );
}
