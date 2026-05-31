namespace Paluwagan.SharedKernel;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
