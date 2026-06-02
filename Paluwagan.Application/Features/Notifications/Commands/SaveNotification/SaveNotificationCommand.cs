using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain.Enums;

namespace Paluwagan.Application.Features.Notifications.Commands.SaveNotification
{
    public sealed record SaveNotificationCommand(
        Guid UserId,
        NotificationType Type,
        string Title,
        string Body,
        string ReferenceId
    ) : ICommand<Guid>;
}
