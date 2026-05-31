using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain;
using Paluwagan.Domain.Entities;
using Paluwagan.Domain.Enums;
using Paluwagan.SharedKernel.Exceptions;

namespace Paluwagan.Application.Features.Register.Commands
{
    internal sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand, RegisterResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RegisterResponse> Handle(RegisterCommand command, CancellationToken ct)
        {
            var existingUser = await _unitOfWork.UserRepository.FindByEmailAsync(command.Email);
            if (existingUser != null)
                throw new BusinessRuleBrokenException("Email is already registered.");

            var role = Enum.Parse<UserRole>(command.Role, ignoreCase: true);
            var user = ApplicationUser.Create(command.FullName, command.Email, role);

            var result = await _unitOfWork.UserRepository.CreateAsync(user, command.Password);
            if (!result.Succeeded)
            {
                var error = result.Errors.First();
                throw new BusinessRuleBrokenException(error.Description);
            }

            await _unitOfWork.UserRepository.AddToRoleAsync(user, role.ToString());

            return new RegisterResponse
            {
                UserId = user.Id,
                AccountId = user.AccountId.Value,
                Email = user.Email!
            };
        }
    }
}
