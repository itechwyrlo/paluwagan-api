using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Logging;
using Paluwagan.Domain.Services;

namespace Paluwagan.Infrastructure.Services
{
    public sealed class FirebaseNotificationService(ILogger<FirebaseNotificationService> logger) : INotificationService
    {
        public async Task<NotificationSendResult> SendPaymentPaidNotificationAsync(
            string fcmToken,
            string groupName,
            int round,
            CancellationToken cancellationToken = default)
        {
            var message = new Message
            {
                Token = fcmToken,
                Notification = new Notification
                {
                    Title = "Payment Confirmed",
                    Body = $"Your Round {round} contribution for \"{groupName}\" has been marked as paid."
                },
                Data = new Dictionary<string, string>
                {
                    { "type", "payment_confirmed" },
                    { "groupName", groupName },
                    { "round", round.ToString() }
                }
            };

            try
            {
                await FirebaseMessaging.DefaultInstance
                    .SendAsync(message, cancellationToken)
                    .ConfigureAwait(false);

                return NotificationSendResult.Sent;
            }
            catch (FirebaseMessagingException fex) when (
                fex.MessagingErrorCode == MessagingErrorCode.Unregistered)
            {
                logger.LogWarning(
                    "FCM token rejected by Firebase (stale/unregistered) for group {GroupName} round {Round}. Token will be cleared.",
                    groupName, round);
                return NotificationSendResult.TokenRejected;
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Failed to send payment confirmed notification for group {GroupName} round {Round}.",
                    groupName, round);
                return NotificationSendResult.Failed;
            }
        }

        public async Task<NotificationSendResult> SendMessageNotificationAsync(
            string fcmToken,
            string senderName,
            string messagePreview,
            string groupId,
            CancellationToken cancellationToken = default)
        {
            var body = messagePreview.Length > 50
                ? messagePreview.Substring(0, 50) + "..."
                : messagePreview;

            var message = new Message
            {
                Token = fcmToken,
                Notification = new Notification
                {
                    Title = senderName,
                    Body = body
                },
                Data = new Dictionary<string, string>
                {
                    { "type", "new_message" },
                    { "groupId", groupId }
                }
            };

            try
            {
                await FirebaseMessaging.DefaultInstance
                    .SendAsync(message, cancellationToken)
                    .ConfigureAwait(false);

                return NotificationSendResult.Sent;
            }
            catch (FirebaseMessagingException fex) when (
                fex.MessagingErrorCode == MessagingErrorCode.Unregistered)
            {
                logger.LogWarning(
                    "FCM token rejected by Firebase (stale/unregistered) for group {GroupId}. Token will be cleared.",
                    groupId);
                return NotificationSendResult.TokenRejected;
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Failed to send FCM message notification for group {GroupId}.",
                    groupId);
                return NotificationSendResult.Failed;
            }
        }
    }
}
