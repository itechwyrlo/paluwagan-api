using System.Threading;
using System.Threading.Tasks;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Mappings;
using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain;
using Paluwagan.Domain.Services;
using Paluwagan.SharedKernel.Exceptions;

namespace Paluwagan.Application.Features.Profile.Commands
{
    internal sealed class UpdatePaymentDetailsCommandHandler(IUnitOfWork unitOfWork, IUserContextService userContext)
        : ICommandHandler<UpdatePaymentDetailsCommand, ProfileResponse>
    {
        public async Task<ProfileResponse> Handle(UpdatePaymentDetailsCommand command, CancellationToken cancellationToken)
        {
            var userId = userContext.GetCurrentUserId();

            var user = await unitOfWork.UserRepository
                .GetByIdAsync(userId)
                .ConfigureAwait(false)
                ?? throw new NotFoundException($"User {userId} was not found.");

            user.UpdatePaymentAccounts(command.GCashNumber, command.MayaNumber);

            await unitOfWork.CompleteAsync(cancellationToken).ConfigureAwait(false);

            return user.ToProfileResponse();
        }
    }
}
