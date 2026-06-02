using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain;
using Paluwagan.Domain.Services;
using Paluwagan.SharedKernel.Exceptions;

namespace Paluwagan.Application.Features.Notifications.Commands.MarkNotificationAsRead
{
    internal sealed class MarkNotificationAsReadCommandHandler(
        IUnitOfWork unitOfWork,
        IUserContextService userContext)
        : ICommandHandler<MarkNotificationAsReadCommand>
    {
        public async Task Handle(MarkNotificationAsReadCommand command, CancellationToken cancellationToken)
        {
            var userId = userContext.GetCurrentUserId();

            var notification = await unitOfWork.NotificationRepository
                .GetByIdForUserAsync(command.NotificationId, userId, cancellationToken)
                .ConfigureAwait(false)
                ?? throw new NotFoundException($"Notification {command.NotificationId} was not found.");

            notification.MarkAsRead();

            await unitOfWork.CompleteAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
