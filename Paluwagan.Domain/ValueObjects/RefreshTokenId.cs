using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Paluwagan.Domain.ValueObjects
{
    [NotMapped]
    public sealed record RefreshTokenId(Guid value);
}