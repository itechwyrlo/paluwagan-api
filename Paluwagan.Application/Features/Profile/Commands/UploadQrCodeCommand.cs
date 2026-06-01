using Paluwagan.Application.DTOs;
using Paluwagan.Application.Messaging.Abstractions;

namespace Paluwagan.Application.Features.Profile.Commands
{
    public sealed record UploadQrCodeCommand(
        byte[] FileBytes,
        string FileName,
        string ContentType
    ) : ICommand<ProfileResponse>;
}
