using System.Threading.Tasks;
using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain;
using Paluwagan.Domain.Services;
using Paluwagan.Domain.ValueObjects;
using Paluwagan.SharedKernel.Exceptions;

namespace Paluwagan.Application.Features.Members.Commands
{
    internal sealed class AddMemberCommandHandler : ICommandHandler<AddMemberCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContext;

        public AddMemberCommandHandler(IUnitOfWork unitOfWork, IUserContextService userContext)
        {
            _unitOfWork = unitOfWork;
            _userContext = userContext;
        }

        public async Task Handle(AddMemberCommand command, CancellationToken cancellationToken)
        {
            var requestingUserId = _userContext.GetCurrentUserId();

            var invitedUser = await _unitOfWork.UserRepository.FindByAccountIdAsync(command.AccountId);
            if (invitedUser == null)
                throw new NotFoundException($"No user found with account ID {command.AccountId}.");

            var group = await _unitOfWork.GroupRepository.GetAsync(
                predicate: g => g.Id == new GroupId(command.GroupId),
                includes: g => g.Members);

            if (group == null)
                throw new NotFoundException($"Group with ID {command.GroupId} was not found.");

            group.AddMember(requestingUserId, invitedUser.Id, command.SlotNumber);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
