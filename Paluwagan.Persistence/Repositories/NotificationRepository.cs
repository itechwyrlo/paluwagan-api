using Microsoft.EntityFrameworkCore;
using Paluwagan.Domain.Entities;
using Paluwagan.Domain.Repositories;
using Paluwagan.Domain.ValueObjects;
using Paluwagan.GenericRepository.EntityFramework;
using Paluwagan.Persistence.Data;

namespace Paluwagan.Persistence.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(PaluwaganDbContext context) : base(context) { }

        public async Task<IReadOnlyList<Notification>> GetByUserIdAsync(Guid userId, CancellationToken ct)
        {
            return await Context.Set<Notification>()
                .AsNoTracking()
                .Where(n => n.UserId == userId)
                .OrderBy(n => n.IsRead)
                .ThenByDescending(n => n.CreatedAt)
                .ToListAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task<Notification?> GetByIdForUserAsync(Guid notificationId, Guid userId, CancellationToken ct)
        {
            var id = new NotificationId(notificationId);
            return await Context.Set<Notification>()
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId, ct)
                .ConfigureAwait(false);
        }

        public async Task MarkAllAsReadAsync(Guid userId, CancellationToken ct)
        {
            await Context.Set<Notification>()
                .Where(n => n.UserId == userId && !n.IsRead)
                .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true), ct)
                .ConfigureAwait(false);
        }
    }
}
