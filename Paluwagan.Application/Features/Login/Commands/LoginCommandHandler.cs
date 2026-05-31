using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain;
using Paluwagan.Domain.Services;
using Paluwagan.SharedKernel.Exceptions;

namespace Paluwagan.Application.Features.Login.Commands
{
    internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, AuthResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly ICookieService _cookieService;

        public LoginCommandHandler(ICookieService cookieService, IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _cookieService = cookieService;
        }

        public async Task<AuthResponse> Handle(LoginCommand command, CancellationToken ct)
        {
            var user = await _unitOfWork.UserRepository.FindByEmailAsync(command.Email);
            if (user == null)
                throw new UnauthorizedException("Invalid email or password.");

            var passwordResult = await _unitOfWork.UserRepository.CheckPasswordAsync(user, command.Password);
            if (!passwordResult)
                throw new UnauthorizedException("Invalid email or password.");

            var role = user.Role.ToString();


            var accessToken = _tokenService.GenerateAccessToken(user, role);
            var rawRefreshToken = _tokenService.GenerateRefreshTokenRaw();

            _cookieService.SetRefreshTokenCookie(rawRefreshToken);

            user.AddRefreshToken(_tokenService.HashToken(rawRefreshToken), DateTime.UtcNow);

            await _unitOfWork.CompleteAsync();

            return new AuthResponse
            {
                AccessToken = accessToken,
                UserId = user.Id
            };
        }
    }
}