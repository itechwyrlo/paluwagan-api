using System.ComponentModel.DataAnnotations;

namespace Paluwagan.API.Dtos
{
    public record UpdatePaymentDetailsRequest(
        [RegularExpression(@"^(09|\+639)\d{9}$", ErrorMessage = "GCash number must be a valid Philippine mobile number (e.g. 09XXXXXXXXX)")]
        string? GCashNumber,

        [RegularExpression(@"^(09|\+639)\d{9}$", ErrorMessage = "Maya number must be a valid Philippine mobile number (e.g. 09XXXXXXXXX)")]
        string? MayaNumber
    );
}
