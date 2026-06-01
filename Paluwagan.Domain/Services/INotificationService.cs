using System.Threading;
using System.Threading.Tasks;

namespace Paluwagan.Domain.Services
{
    public interface INotificationService
    {
        Task SendMessageNotificationAsync(
            string fcmToken,
            string senderName,
            string messagePreview,
            string groupId,
            CancellationToken cancellationToken = default);

        Task SendPaymentPaidNotificationAsync(
            string fcmToken,
            string groupName,
            int round,
            CancellationToken cancellationToken = default);
    }
}
