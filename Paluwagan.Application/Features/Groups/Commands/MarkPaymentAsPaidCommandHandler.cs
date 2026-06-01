using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain;
using Paluwagan.Domain.Services;
using Paluwagan.Domain.ValueObjects;
using Paluwagan.SharedKernel.Exceptions;

namespace Paluwagan.Application.Features.Groups.Commands
{
    internal sealed class MarkPaymentAsPaidCommandHandler(
        IUnitOfWork unitOfWork,
        IUserContextService userContext,
        INotificationService notificationService,
        ILogger<MarkPaymentAsPaidCommandHandler> logger)
        : ICommandHandler<MarkPaymentAsPaidCommand, MarkPaymentAsPaidResponse>
    {
        public async Task<MarkPaymentAsPaidResponse> Handle(MarkPaymentAsPaidCommand command, CancellationToken cancellationToken)
        {
            var organizerId = userContext.GetCurrentUserId();

            var group = await unitOfWork.GroupRepository
                .GetByIdWithDetailsAsync(new GroupId(command.GroupId))
                .ConfigureAwait(false)
                ?? throw new NotFoundException($"Group {command.GroupId} was not found.");

            group.MarkPaymentAsPaid(organizerId, command.MemberId, command.Round);

            await unitOfWork.CompleteAsync(cancellationToken).ConfigureAwait(false);

            var payment = group.Payments.First(p => p.MemberId == command.MemberId && p.Round == command.Round);

            try
            {
                var member = await unitOfWork.UserRepository
                    .GetByIdAsync(command.MemberId)
                    .ConfigureAwait(false);

                if (member is null)
                {
                    logger.LogWarning(
                        "Push notification skipped: member {MemberId} not found.",
                        command.MemberId);
                }
                else if (member.FcmToken is null)
                {
                    logger.LogWarning(
                        "Push notification skipped: member {MemberId} has no FCM token registered.",
                        command.MemberId);
                }
                else
                {
                    var result = await notificationService.SendPaymentPaidNotificationAsync(
                        member.FcmToken,
                        group.Name,
                        command.Round,
                        cancellationToken).ConfigureAwait(false);

                    if (result == NotificationSendResult.TokenRejected)
                    {
                        member.ClearFcmToken();
                        await unitOfWork.CompleteAsync(cancellationToken).ConfigureAwait(false);
                        logger.LogWarning(
                            "Stale FCM token cleared for member {MemberId}. They must re-enable notifications.",
                            command.MemberId);
                    }
                    else if (result == NotificationSendResult.Sent)
                    {
                        logger.LogInformation(
                            "Push notification sent to member {MemberId} for payment round {Round} in group {GroupId}.",
                            command.MemberId, command.Round, command.GroupId);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Unexpected error sending payment notification for member {MemberId} in group {GroupId}.",
                    command.MemberId, command.GroupId);
            }

            return new MarkPaymentAsPaidResponse(payment.MemberId, payment.Round, payment.IsPaid, payment.PaidAt);
        }
    }
}
