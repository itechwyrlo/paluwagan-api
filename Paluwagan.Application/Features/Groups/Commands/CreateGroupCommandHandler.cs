using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Features.Groups.Commands;
using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain;
using Paluwagan.Domain.Entities;
using Paluwagan.Domain.Enums;
using Paluwagan.Domain.Services;

namespace Paluwagan.Application.Features.Groups.Commands
{
    internal sealed class CreateGroupCommandHandler : ICommandHandler<CreateGroupCommand, CreateGroupResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContext;


        public CreateGroupCommandHandler(IUnitOfWork unitOfWork, IUserContextService userContext)
        {
            _unitOfWork = unitOfWork;
            _userContext = userContext;
        }

        public async Task<CreateGroupResponse> Handle(CreateGroupCommand command, CancellationToken ct)
        {
            var organizerId = _userContext.GetCurrentUserId();
            
            var group = Group.Create(
                command.Name,
                command.ContributionAmount,
                command.Schedule,
                command.NumberOfSlots,
                command.StartDate,
                organizerId);

            _unitOfWork.GroupRepository.Add(group);

            var user = await _unitOfWork.UserRepository.GetByIdAsync(organizerId);
            if (user is not null && user.Role != UserRole.Organizer)
            {
                user.PromoteToOrganizer();
                await _unitOfWork.UserRepository.AddToRoleAsync(user, UserRole.Organizer.ToString());
            }

            await _unitOfWork.CompleteAsync(ct);

            return new CreateGroupResponse { GroupId = group.Id };
        }
    }
}
