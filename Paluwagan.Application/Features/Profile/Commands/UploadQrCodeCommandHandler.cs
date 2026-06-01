using Paluwagan.Application.DTOs;
using Paluwagan.Application.Mappings;
using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain;
using Paluwagan.Domain.Services;
using Paluwagan.SharedKernel.Exceptions;

namespace Paluwagan.Application.Features.Profile.Commands
{
    internal sealed class UploadQrCodeCommandHandler(
        IUnitOfWork unitOfWork,
        IUserContextService userContext,
        IStorageService storageService)
        : ICommandHandler<UploadQrCodeCommand, ProfileResponse>
    {
        public async Task<ProfileResponse> Handle(UploadQrCodeCommand command, CancellationToken cancellationToken)
        {
            var userId = userContext.GetCurrentUserId();

            var user = await unitOfWork.UserRepository
                .GetByIdAsync(userId)
                .ConfigureAwait(false)
                ?? throw new NotFoundException($"User {userId} was not found.");

            var url = await storageService
                .UploadAsync(command.FileName, command.FileBytes, command.ContentType, cancellationToken)
                .ConfigureAwait(false);

            user.UpdateQrCode(url);

            await unitOfWork.CompleteAsync(cancellationToken).ConfigureAwait(false);

            return user.ToProfileResponse();
        }
    }
}
