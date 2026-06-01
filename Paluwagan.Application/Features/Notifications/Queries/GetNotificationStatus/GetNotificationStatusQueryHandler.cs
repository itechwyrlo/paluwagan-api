using System;
using System.Threading;
using System.Threading.Tasks;
using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain;
using Paluwagan.Domain.Services;
using Paluwagan.SharedKernel.Exceptions;

namespace Paluwagan.Application.Features.Notifications.Queries.GetNotificationStatus
{
    internal sealed class GetNotificationStatusQueryHandler(
        IUnitOfWork unitOfWork,
        IUserContextService userContext)
        : IQueryHandler<GetNotificationStatusQuery, NotificationStatusResponse>
    {
        public async Task<NotificationStatusResponse> Handle(GetNotificationStatusQuery query, CancellationToken cancellationToken)
        {
            var lookupId = query.TargetUserId ?? userContext.GetCurrentUserId();

            var user = await unitOfWork.UserRepository
                .GetByIdAsync(lookupId)
                .ConfigureAwait(false)
                ?? throw new NotFoundException($"User {lookupId} was not found.");

            string? preview = null;
            if (user.FcmToken is not null)
            {
                preview = user.FcmToken.Length > 20
                    ? user.FcmToken[..20] + "..."
                    : user.FcmToken;
            }

            return new NotificationStatusResponse(user.Id, user.FcmToken is not null, preview);
        }
    }
}
