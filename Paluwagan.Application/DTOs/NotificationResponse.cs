namespace Paluwagan.Application.DTOs
{
    public sealed record NotificationResponse(
        Guid Id,
        string Type,
        string Title,
        string Body,
        bool IsRead,
        DateTimeOffset CreatedAt,
        string ReferenceId
    );
}
