using Paluwagan.Application.Messaging.Abstractions;

namespace Paluwagan.Application.Features.Notifications.Commands.MarkNotificationAsRead
{
    public sealed record MarkNotificationAsReadCommand(Guid NotificationId) : ICommand;
}
