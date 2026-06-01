using Paluwagan.Application.DTOs;
using Paluwagan.Domain.Entities;

namespace Paluwagan.Application.Mappings
{
    public static class MessageMappings
    {
        public static MessageResponse ToMessageResponse(this Message message, string senderName) =>
            new(
                Id: message.Id.Value.ToString(),
                GroupId: message.GroupId.Value.ToString(),
                SenderId: message.SenderId.ToString(),
                SenderName: senderName,
                ReceiverId: message.ReceiverId.ToString(),
                Text: message.Text,
                ImageUrl: message.ImageUrl,
                SentAt: message.SentAt,
                IsRead: message.IsRead
            );
    }
}
