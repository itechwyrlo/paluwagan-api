using System;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Messaging.Abstractions;

namespace Paluwagan.Application.Features.Messages.Commands.SendMessage
{
    public sealed record SendMessageCommand(
        Guid GroupId,
        Guid ReceiverId,
        string? Text,
        string? ImageUrl
    ) : ICommand<MessageResponse>;
}
