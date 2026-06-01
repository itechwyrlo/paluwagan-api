using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Paluwagan.Domain.Entities;
using Paluwagan.GenericRepository.Abstractions;

namespace Paluwagan.Domain.Repositories
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<IReadOnlyList<Message>> GetConversationAsync(Guid groupId, Guid userId1, Guid userId2, CancellationToken ct);
    }
}
