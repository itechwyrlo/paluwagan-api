namespace Paluwagan.Application.DTOs
{
    public sealed record ProfileResponse(
        string AccountId,
        string FullName,
        string Email,
        string Role,
        string? GCashNumber,
        string? MayaNumber
    );
}
