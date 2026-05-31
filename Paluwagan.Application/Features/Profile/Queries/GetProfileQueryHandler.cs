using System.Threading;
using System.Threading.Tasks;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Mappings;
using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain;
using Paluwagan.Domain.Services;
using Paluwagan.SharedKernel.Exceptions;

namespace Paluwagan.Application.Features.Profile.Queries
{
    internal sealed class GetProfileQueryHandler(IUnitOfWork unitOfWork, IUserContextService userContext)
        : IQueryHandler<GetProfileQuery, ProfileResponse>
    {
        public async Task<ProfileResponse> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = userContext.GetCurrentUserId();

            var user = await unitOfWork.UserRepository
                .GetByIdAsync(userId)
                .ConfigureAwait(false)
                ?? throw new NotFoundException($"User {userId} was not found.");

            return user.ToProfileResponse();
        }
    }
}
