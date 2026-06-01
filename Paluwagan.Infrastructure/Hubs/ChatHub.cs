using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Paluwagan.Infrastructure.Hubs
{
    [Authorize]
    public sealed class ChatHub : Hub
    {
        public async Task JoinGroupChat(string groupId)
            => await Groups.AddToGroupAsync(Context.ConnectionId, $"group-{groupId}");

        public async Task LeaveGroupChat(string groupId)
            => await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"group-{groupId}");
    }
}
