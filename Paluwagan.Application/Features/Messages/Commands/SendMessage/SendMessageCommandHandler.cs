using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Features.Notifications.Commands.SaveNotification;
using Paluwagan.Application.Mappings;
using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain;
using Paluwagan.Domain.Entities;
using Paluwagan.Domain.Enums;
using Paluwagan.Domain.Services;
using Paluwagan.Domain.ValueObjects;
using Paluwagan.SharedKernel.Exceptions;

namespace Paluwagan.Application.Features.Messages.Commands.SendMessage
{
    internal sealed class SendMessageCommandHandler(
        IUnitOfWork unitOfWork,
        IUserContextService userContext,
        IChatNotifier chatNotifier,
        INotificationService notificationService,
        ISender mediator,
        ILogger<SendMessageCommandHandler> logger)
        : ICommandHandler<SendMessageCommand, MessageResponse>
    {
        public async Task<MessageResponse> Handle(SendMessageCommand command, CancellationToken cancellationToken)
        {
            var senderId = userContext.GetCurrentUserId();

            var senderUser = await unitOfWork.UserRepository
                .GetByIdAsync(senderId)
                .ConfigureAwait(false)
                ?? throw new NotFoundException($"User {senderId} was not found.");

            var message = Message.Create(
                new GroupId(command.GroupId),
                senderId,
                command.ReceiverId,
                command.Text,
                command.ImageUrl);

            unitOfWork.MessageRepository.Add(message);
            await unitOfWork.CompleteAsync(cancellationToken).ConfigureAwait(false);

            var response = message.ToMessageResponse(senderUser.FullName);

            var messagePreview = command.Text is not null ? command.Text : "Sent an image";

            var notificationId = await mediator.Send(
                new SaveNotificationCommand(
                    command.ReceiverId,
                    NotificationType.NewMessage,
                    senderUser.FullName,
                    messagePreview,
                    command.GroupId.ToString()),
                cancellationToken).ConfigureAwait(false);

            try
            {
                await chatNotifier.NotifyUserAsync(
                    command.ReceiverId,
                    notificationId,
                    NotificationType.NewMessage.ToString(),
                    senderUser.FullName,
                    messagePreview,
                    command.GroupId.ToString(),
                    DateTimeOffset.UtcNow,
                    cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "SignalR ReceiveNotification failed for receiver {ReceiverId}.", command.ReceiverId);
            }

            try
            {
                await chatNotifier.NotifyGroupAsync(message, senderUser.FullName, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "SignalR ReceiveMessage failed for group {GroupId}.", command.GroupId);
            }

            try
            {
                var receiver = await unitOfWork.UserRepository
                    .GetByIdAsync(command.ReceiverId)
                    .ConfigureAwait(false);

                if (receiver is null)
                {
                    logger.LogWarning(
                        "Push notification skipped: receiver {ReceiverId} not found.",
                        command.ReceiverId);
                }
                else if (receiver.FcmToken is null)
                {
                    logger.LogWarning(
                        "Push notification skipped: receiver {ReceiverId} has no FCM token registered.",
                        command.ReceiverId);
                }
                else
                {
                    var result = await notificationService.SendMessageNotificationAsync(
                        receiver.FcmToken,
                        senderUser.FullName,
                        messagePreview,
                        command.GroupId.ToString(),
                        cancellationToken).ConfigureAwait(false);

                    if (result == NotificationSendResult.TokenRejected)
                    {
                        receiver.ClearFcmToken();
                        await unitOfWork.CompleteAsync(cancellationToken).ConfigureAwait(false);
                        logger.LogWarning(
                            "Stale FCM token cleared for receiver {ReceiverId}. They must re-enable notifications.",
                            command.ReceiverId);
                    }
                    else if (result == NotificationSendResult.Sent)
                    {
                        logger.LogInformation(
                            "Push notification sent to receiver {ReceiverId} for group {GroupId}.",
                            command.ReceiverId, command.GroupId);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error sending push notification for group {GroupId}.", command.GroupId);
            }

            return response;
        }
    }
}
