using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain;
using Paluwagan.Domain.Services;
using Paluwagan.SharedKernel.Exceptions;

namespace Paluwagan.Application.Features.RefreshToken.Commands
{
    internal sealed class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly ICookieService _cookieService;

        public RefreshTokenCommandHandler(IUnitOfWork unitOfWork, ITokenService tokenService, ICookieService cookieService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _cookieService = cookieService;
        }

        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand command, CancellationToken ct)
        {
            var rawToken = _cookieService.GetRefreshTokenFromCookie();
            if (string.IsNullOrEmpty(rawToken))
                throw new UnauthorizedException("Invalid or missing refresh token.");

            var hashedToken = _tokenService.HashToken(rawToken);

            var user = await _unitOfWork.UserRepository.FindByRefreshTokenAsync(hashedToken)
                ?? throw new UnauthorizedException("Invalid or missing refresh token.");

            var newRawToken = _tokenService.GenerateRefreshTokenRaw();
            var hashedNewToken = _tokenService.HashToken(newRawToken);
            var utcNow = DateTime.UtcNow;

            user.RotateRefreshToken(hashedToken, hashedNewToken, utcNow);

            var role = user.Role.ToString();
            var newAccessToken = _tokenService.GenerateAccessToken(user, role);

            _cookieService.SetRefreshTokenCookie(newRawToken);

            await _unitOfWork.CompleteAsync(ct);

            return new RefreshTokenResponse
            {
                AccessToken = newAccessToken,
                UserId = user.Id
            };
        }
    }

}