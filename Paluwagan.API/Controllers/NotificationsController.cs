using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Paluwagan.API.Dtos;
using Paluwagan.Application.Features.Notifications.Commands.SaveFcmToken;
using Paluwagan.Application.Features.Notifications.Queries.GetNotificationStatus;
using Paluwagan.Infrastructure.Configurations;

namespace Paluwagan.API.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    [Authorize(Policy = Policies.OrganizerOrMember)]
    public class NotificationsController : Controller
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("fcm-token")]
        public async Task<IActionResult> SaveFcmToken([FromBody] SaveFcmTokenRequest request, CancellationToken ct)
        {
            await _mediator.Send(new SaveFcmTokenCommand(request.Token), ct);
            return NoContent();
        }

        /// <summary>
        /// Returns the FCM token registration status for the current user (or a specific user by ID).
        /// Use this to diagnose why push notifications are not being received.
        /// Call GET /api/notifications/diagnose to check yourself.
        /// Call GET /api/notifications/diagnose?userId={guid} to check another user.
        /// </summary>
        [HttpGet("diagnose")]
        public async Task<IActionResult> Diagnose([FromQuery] Guid? userId, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetNotificationStatusQuery(userId), ct);
            return Ok(result);
        }
    }
}
