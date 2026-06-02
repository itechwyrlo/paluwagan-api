using Paluwagan.Application.DTOs;
using Paluwagan.Domain.Entities;

namespace Paluwagan.Application.Mappings
{
    public static class NotificationMappings
    {
        public static NotificationResponse ToNotificationResponse(this Notification notification) =>
            new(
                Id: notification.Id.Value,
                Type: notification.Type.ToString(),
                Title: notification.Title,
                Body: notification.Body,
                IsRead: notification.IsRead,
                CreatedAt: notification.CreatedAt,
                ReferenceId: notification.ReferenceId
            );
    }
}
