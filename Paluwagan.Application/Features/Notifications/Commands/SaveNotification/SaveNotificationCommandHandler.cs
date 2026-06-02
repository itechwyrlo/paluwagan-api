using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain;
using Paluwagan.Domain.Entities;

namespace Paluwagan.Application.Features.Notifications.Commands.SaveNotification
{
    internal sealed class SaveNotificationCommandHandler(IUnitOfWork unitOfWork)
        : ICommandHandler<SaveNotificationCommand, Guid>
    {
        public async Task<Guid> Handle(SaveNotificationCommand command, CancellationToken cancellationToken)
        {
            var notification = Notification.Create(
                command.UserId,
                command.Type,
                command.Title,
                command.Body,
                command.ReferenceId);

            unitOfWork.NotificationRepository.Add(notification);
            await unitOfWork.CompleteAsync(cancellationToken).ConfigureAwait(false);

            return notification.Id.Value;
        }
    }
}
