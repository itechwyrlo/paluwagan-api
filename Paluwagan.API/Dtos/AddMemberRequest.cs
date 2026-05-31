using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Paluwagan.API.Dtos
{
    public record AddMemberRequest(
        [Required(ErrorMessage = "Account ID is required")]
        string AccountId,
        [Required(ErrorMessage = "Slot number is required")]
        int SlotNumber
    );
}
