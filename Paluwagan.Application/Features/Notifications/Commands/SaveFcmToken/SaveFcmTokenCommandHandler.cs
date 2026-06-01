using System.Threading;
using System.Threading.Tasks;
using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain;
using Paluwagan.Domain.Services;
using Paluwagan.SharedKernel.Exceptions;

namespace Paluwagan.Application.Features.Notifications.Commands.SaveFcmToken
{
    internal sealed class SaveFcmTokenCommandHandler(
        IUnitOfWork unitOfWork,
        IUserContextService userContext)
        : ICommandHandler<SaveFcmTokenCommand>
    {
        public async Task Handle(SaveFcmTokenCommand command, CancellationToken cancellationToken)
        {
            var userId = userContext.GetCurrentUserId();

            var user = await unitOfWork.UserRepository
                .GetByIdAsync(userId)
                .ConfigureAwait(false)
                ?? throw new NotFoundException($"User {userId} was not found.");

            user.UpdateFcmToken(command.Token);

            await unitOfWork.CompleteAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
