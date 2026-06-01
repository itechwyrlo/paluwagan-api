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
        public async Task SendPaymentPaidNotificationAsync(
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
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send payment confirmed notification to token {Token} for group {GroupName} round {Round}.", fcmToken, groupName, round);
            }
        }

        public async Task SendMessageNotificationAsync(
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
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send FCM notification to token {Token} for group {GroupId}.", fcmToken, groupId);
            }
        }
    }
}
