using System.Threading;
using System.Threading.Tasks;
using Paluwagan.Domain.Entities;

namespace Paluwagan.Domain.Services
{
    public interface IChatNotifier
    {
        Task NotifyGroupAsync(Message message, string senderName, CancellationToken ct);
    }
}
