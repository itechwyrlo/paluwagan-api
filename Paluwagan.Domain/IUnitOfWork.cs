using Paluwagan.Domain.Repositories;

namespace Paluwagan.Domain;
public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IGroupRepository GroupRepository { get; }
    IMessageRepository MessageRepository { get; }
    INotificationRepository NotificationRepository { get; }

    Task<int> CompleteAsync();
    Task<int> CompleteAsync(CancellationToken cancellationToken);
}
