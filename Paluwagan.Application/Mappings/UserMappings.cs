using Paluwagan.Application.DTOs;
using Paluwagan.Domain.Entities;

namespace Paluwagan.Application.Mappings
{
    public static class UserMappings
    {
        public static ProfileResponse ToProfileResponse(this ApplicationUser user) =>
            new(
                AccountId: user.AccountId.Value,
                FullName: user.FullName,
                Email: user.Email ?? string.Empty,
                Role: user.Role.ToString(),
                GCashNumber: user.GCashNumber,
                MayaNumber: user.MayaNumber,
                QrCodeUrl: user.QrCodeUrl
            );
    }
}
