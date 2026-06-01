using System;

namespace Paluwagan.Application.DTOs
{
    public sealed record MessageResponse(
        string Id,
        string GroupId,
        string SenderId,
        string SenderName,
        string ReceiverId,
        string? Text,
        string? ImageUrl,
        DateTime SentAt,
        bool IsRead
    );
}
