using Paluwagan.Domain.Entities;
using Paluwagan.GenericRepository.Abstractions;

namespace Paluwagan.Domain.Repositories
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<IReadOnlyList<Notification>> GetByUserIdAsync(Guid userId, CancellationToken ct);
        Task<Notification?> GetByIdForUserAsync(Guid notificationId, Guid userId, CancellationToken ct);
        Task MarkAllAsReadAsync(Guid userId, CancellationToken ct);
    }
}
