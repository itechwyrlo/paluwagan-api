using Paluwagan.Application.Messaging.Abstractions;

namespace Paluwagan.Application.Features.Notifications.Commands.SaveFcmToken
{
    public sealed record SaveFcmTokenCommand(string Token) : ICommand;
}
