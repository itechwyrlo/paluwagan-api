using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paluwagan.Domain.Services
{
    public interface IUserContextService
    {
        string GetCurrentJti();
        string GetUserRole();
        Guid GetCurrentUserId();
        bool IsAuthenticated();
    }
}