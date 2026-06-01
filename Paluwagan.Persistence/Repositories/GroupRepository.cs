using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Paluwagan.Domain.Entities;
using Paluwagan.Domain.Repositories;
using Paluwagan.Domain.ValueObjects;
using Paluwagan.GenericRepository.EntityFramework;
using Paluwagan.Persistence.Data;

namespace Paluwagan.Persistence.Repositories
{
    public class GroupRepository : GenericRepository<Group>, IGroupRepository
    {
        public GroupRepository(PaluwaganDbContext context) : base(context) { }

        public async Task<Group?> GetByIdWithDetailsAsync(GroupId groupId)
        {
            return await Context.Set<Group>()
                .Include(g => g.Members)
                .Include(g => g.Payments)
                .FirstOrDefaultAsync(g => g.Id == groupId)
                .ConfigureAwait(false);
        }
    }
}
