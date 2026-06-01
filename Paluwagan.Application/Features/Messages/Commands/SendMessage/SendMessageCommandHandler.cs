using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Mappings;
using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain;
using Paluwagan.Domain.Entities;
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
        ILogger<SendMessageCommandHandler> logger)
        : ICommandHandler<SendMessageCommand, MessageResponse>
    {
        public async Task<MessageResponse> Handle(SendMessageCommand command, CancellationToken cancellationToken)
        {
            var senderId = userContext.GetCurrentUserId();

            var sender = await unitOfWork.UserRepository
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

            var response = message.ToMessageResponse(sender.FullName);

            await chatNotifier.NotifyGroupAsync(message, sender.FullName, cancellationToken).ConfigureAwait(false);

            try
            {
                var receiver = await unitOfWork.UserRepository
                    .GetByIdAsync(command.ReceiverId)
                    .ConfigureAwait(false);

                if (receiver?.FcmToken is not null)
                {
                    var preview = command.Text is not null ? command.Text : "Sent an image";

                    await notificationService.SendMessageNotificationAsync(
                        receiver.FcmToken,
                        sender.FullName,
                        preview,
                        command.GroupId.ToString(),
                        cancellationToken).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send push notification for message in group {GroupId}.", command.GroupId);
            }

            return response;
        }
    }
}
