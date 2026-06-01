using System;
using System.Threading.Tasks;
using Paluwagan.Domain.Entities;
using Paluwagan.Domain.ValueObjects;
using Paluwagan.GenericRepository.Abstractions;

namespace Paluwagan.Domain.Repositories
{
    public interface IGroupRepository : IGenericRepository<Group>
    {
        Task<Group?> GetByIdWithDetailsAsync(GroupId groupId);
    }
}
