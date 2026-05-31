using System.ComponentModel.DataAnnotations.Schema;

namespace Paluwagan.Domain.ValueObjects
{
    [NotMapped]
    public sealed record AccountId(string Value);
}
