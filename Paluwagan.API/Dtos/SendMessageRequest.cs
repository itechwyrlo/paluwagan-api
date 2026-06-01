using System;

namespace Paluwagan.API.Dtos
{
    public sealed record SendMessageRequest(
        Guid GroupId,
        Guid ReceiverId,
        string? Text,
        string? ImageUrl
    );
}
