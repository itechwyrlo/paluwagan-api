using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain;
using Paluwagan.Domain.Services;

namespace Paluwagan.Application.Features.Notifications.Commands.MarkAllNotificationsAsRead
{
    internal sealed class MarkAllNotificationsAsReadCommandHandler(
        IUnitOfWork unitOfWork,
        IUserContextService userContext)
        : ICommandHandler<MarkAllNotificationsAsReadCommand>
    {
        public async Task Handle(MarkAllNotificationsAsReadCommand command, CancellationToken cancellationToken)
        {
            var userId = userContext.GetCurrentUserId();

            await unitOfWork.NotificationRepository
                .MarkAllAsReadAsync(userId, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
