using System;
using System.Threading.Tasks;
using Paluwagan.Domain.Entities;
using Paluwagan.GenericRepository.Abstractions;

namespace Paluwagan.Domain.Repositories
{
    public interface IGroupRepository : IGenericRepository<Group>
    {
        Task<Group?> GetByIdWithDetailsAsync(Guid groupId);
    }
}
