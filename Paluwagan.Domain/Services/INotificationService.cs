using System.Threading;
using System.Threading.Tasks;

namespace Paluwagan.Domain.Services
{
    public interface INotificationService
    {
        Task<NotificationSendResult> SendMessageNotificationAsync(
            string fcmToken,
            string senderName,
            string messagePreview,
            string groupId,
            CancellationToken cancellationToken = default);

        Task<NotificationSendResult> SendPaymentPaidNotificationAsync(
            string fcmToken,
            string groupName,
            int round,
            CancellationToken cancellationToken = default);
    }
}
