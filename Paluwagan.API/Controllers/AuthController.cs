using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Paluwagan.API.Dtos;
using Paluwagan.Application.Features.Login.Commands;
using Paluwagan.Application.Features.Logout.Commands;
using Paluwagan.Application.Features.RefreshToken.Commands;
using Paluwagan.Application.Features.Register.Commands;

namespace Paluwagan.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
        {
            var result = await _mediator.Send(new LoginCommand(request.Email, request.Password));
            return Ok(result);


        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var result = await _mediator.Send(new RefreshTokenCommand());
            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _mediator.Send(new LogoutCommand());
            return NoContent();
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
        {
            var result = await _mediator.Send(new RegisterCommand(
                request.FullName,
                request.Email,
                request.Password,
                request.ConfirmPassword,
                request.Role), ct);
            return StatusCode(StatusCodes.Status201Created, result);
        }
    }
}