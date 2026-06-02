using Microsoft.AspNetCore.Identity;
using Paluwagan.Domain;
using Paluwagan.Domain.Entities;
using Paluwagan.Domain.Repositories;
using Paluwagan.Persistence.Data;
using Paluwagan.Persistence.Repositories;

namespace Paluwagan.Persistence;

public class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
    private readonly PaluwaganDbContext _context;

    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<IGroupRepository> _groupRepository;
    private readonly Lazy<IMessageRepository> _messageRepository;
    private readonly Lazy<INotificationRepository> _notificationRepository;

    public IUserRepository UserRepository => _userRepository.Value;
    public IGroupRepository GroupRepository => _groupRepository.Value;
    public IMessageRepository MessageRepository => _messageRepository.Value;
    public INotificationRepository NotificationRepository => _notificationRepository.Value;
    public UserManager<ApplicationUser> UserManager { get; }
    public RoleManager<IdentityRole<Guid>> RoleManager { get; }

    public UnitOfWork(PaluwaganDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        RoleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));

        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(_context, UserManager));
        _groupRepository = new Lazy<IGroupRepository>(() => new GroupRepository(_context));
        _messageRepository = new Lazy<IMessageRepository>(() => new MessageRepository(_context));
        _notificationRepository = new Lazy<INotificationRepository>(() => new NotificationRepository(_context));
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<int> CompleteAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public ValueTask DisposeAsync()
    {
        return _context.DisposeAsync();
    }
}
