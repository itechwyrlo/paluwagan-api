using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Paluwagan.Domain.Entities;
using Paluwagan.Domain.Services;
using Paluwagan.Infrastructure.Hubs;

namespace Paluwagan.Infrastructure.Services
{
    public sealed class SignalRChatNotifier(IHubContext<ChatHub> hubContext) : IChatNotifier
    {
        public async Task NotifyGroupAsync(Message message, string senderName, CancellationToken ct)
        {
            var groupName = $"group-{message.GroupId.Value}";

            var payload = new
            {
                id = message.Id.Value.ToString(),
                groupId = message.GroupId.Value.ToString(),
                senderId = message.SenderId.ToString(),
                senderName,
                receiverId = message.ReceiverId.ToString(),
                text = message.Text,
                imageUrl = message.ImageUrl,
                sentAt = message.SentAt,
                isRead = message.IsRead
            };

            await hubContext.Clients
                .Group(groupName)
                .SendAsync("ReceiveMessage", payload, ct)
                .ConfigureAwait(false);
        }
    }
}
