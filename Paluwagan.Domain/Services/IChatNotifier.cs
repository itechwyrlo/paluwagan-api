using System;
using System.Threading;
using System.Threading.Tasks;
using Paluwagan.Domain.Entities;

namespace Paluwagan.Domain.Services
{
    public interface IChatNotifier
    {
        Task NotifyGroupAsync(Message message, string senderName, CancellationToken ct);
        Task NotifyUserAsync(Guid userId, Guid notificationId, string type, string title, string body, string referenceId, DateTimeOffset createdAt, CancellationToken ct);
    }
}
