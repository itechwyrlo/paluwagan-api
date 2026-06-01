using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Paluwagan.Domain.Entities;
using Paluwagan.Domain.Repositories;
using Paluwagan.Domain.ValueObjects;
using Paluwagan.GenericRepository.EntityFramework;
using Paluwagan.Persistence.Data;

namespace Paluwagan.Persistence.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(PaluwaganDbContext context) : base(context) { }

        public async Task<IReadOnlyList<Message>> GetConversationAsync(
            Guid groupId, Guid userId1, Guid userId2, CancellationToken ct)
        {
            var groupIdValue = new GroupId(groupId);

            return await Context.Set<Message>()
                .AsNoTracking()
                .Where(m =>
                    m.GroupId == groupIdValue &&
                    ((m.SenderId == userId1 && m.ReceiverId == userId2) ||
                     (m.SenderId == userId2 && m.ReceiverId == userId1)))
                .OrderBy(m => m.SentAt)
                .ToListAsync(ct)
                .ConfigureAwait(false);
        }
    }
}
