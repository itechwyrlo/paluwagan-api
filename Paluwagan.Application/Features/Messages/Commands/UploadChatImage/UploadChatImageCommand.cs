using Paluwagan.Application.Messaging.Abstractions;

namespace Paluwagan.Application.Features.Messages.Commands.UploadChatImage
{
    public sealed record UploadChatImageCommand(
        byte[] FileBytes,
        string FileName,
        string ContentType
    ) : ICommand<string>;
}
