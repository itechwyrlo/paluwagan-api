using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paluwagan.Domain.ValueObjects
{
    [NotMapped]
    public sealed record GroupId(Guid Value);
}
