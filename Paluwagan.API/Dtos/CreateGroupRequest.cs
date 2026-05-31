using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Paluwagan.Domain.Enums;

namespace Paluwagan.API.Dtos
{
    public record CreateGroupRequest(
        [Required(ErrorMessage = "Group name is required")]
        string Name,

        [Required(ErrorMessage = "Contribution amount is required")]
        decimal ContributionAmount,

        [Required(ErrorMessage = "Payment schedule is required")]
        PaymentSchedule Schedule,

        [Required(ErrorMessage = "Number of slots is required")]
        int NumberOfSlots,

        [Required(ErrorMessage = "Start date is required")]
        DateTime StartDate
    );
}
