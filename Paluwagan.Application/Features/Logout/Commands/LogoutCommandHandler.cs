using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain;
using Paluwagan.Domain.Services;

namespace Paluwagan.Application.Features.Logout.Commands
{
    internal sealed class LogoutCommandHandler : ICommandHandler<LogoutCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICookieService _cookieService;
        private readonly IUserContextService _userContext;
        private readonly ITokenService _tokenService;

        public LogoutCommandHandler(
            IUnitOfWork unitOfWork,
            ICookieService cookieService,
            IUserContextService userContext,
            ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _cookieService = cookieService;
            _userContext = userContext;
            _tokenService = tokenService;
        }

        public async Task Handle(LogoutCommand command, CancellationToken cancellationToken)
        {
            var userId = _userContext.GetCurrentUserId();
            var user = await _unitOfWork.UserRepository.GetAsync(
            predicate: u => u.Id == userId,
            includes: u => u.RefreshTokens);

            if (user == null) return;

            var refreshTokenValue = _cookieService.GetRefreshTokenFromCookie();

            if (!string.IsNullOrEmpty(refreshTokenValue))
            {
                user.RevokeSpecificToken(_tokenService.HashToken(refreshTokenValue));
            }

            var jti = _userContext.GetCurrentJti();
            await _tokenService.BlacklistJtiAsync(jti, cancellationToken);

            _cookieService.DeleteRefreshTokenCookie();

            user.ClearFcmToken();
            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}