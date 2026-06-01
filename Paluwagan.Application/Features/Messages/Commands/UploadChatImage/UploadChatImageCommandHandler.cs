using System.Threading;
using System.Threading.Tasks;
using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain.Services;

namespace Paluwagan.Application.Features.Messages.Commands.UploadChatImage
{
    internal sealed class UploadChatImageCommandHandler(IStorageService storageService)
        : ICommandHandler<UploadChatImageCommand, string>
    {
        public async Task<string> Handle(UploadChatImageCommand command, CancellationToken cancellationToken)
        {
            return await storageService
                .UploadAsync(command.FileName, command.FileBytes, command.ContentType, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
