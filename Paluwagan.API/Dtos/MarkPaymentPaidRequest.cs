using System.ComponentModel.DataAnnotations;

namespace Paluwagan.API.Dtos
{
    public record MarkPaymentPaidRequest(
        [Required(ErrorMessage = "Round is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Round must be at least 1")]
        int Round
    );
}
