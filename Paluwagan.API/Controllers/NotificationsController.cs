using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Paluwagan.API.Dtos;
using Paluwagan.Application.Features.Notifications.Commands.MarkAllNotificationsAsRead;
using Paluwagan.Application.Features.Notifications.Commands.MarkNotificationAsRead;
using Paluwagan.Application.Features.Notifications.Commands.SaveFcmToken;
using Paluwagan.Application.Features.Notifications.Queries.GetNotificationStatus;
using Paluwagan.Application.Features.Notifications.Queries.GetNotifications;
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

        [HttpGet]
        public async Task<IActionResult> GetNotifications(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetNotificationsQuery(), ct);
            return Ok(result);
        }

        [HttpPut("read-all")]
        public async Task<IActionResult> MarkAllAsRead(CancellationToken ct)
        {
            await _mediator.Send(new MarkAllNotificationsAsReadCommand(), ct);
            return NoContent();
        }

        [HttpPut("{id:guid}/read")]
        public async Task<IActionResult> MarkAsRead(Guid id, CancellationToken ct)
        {
            await _mediator.Send(new MarkNotificationAsReadCommand(id), ct);
            return NoContent();
        }

        [HttpPost("fcm-token")]
        public async Task<IActionResult> SaveFcmToken([FromBody] SaveFcmTokenRequest request, CancellationToken ct)
        {
            await _mediator.Send(new SaveFcmTokenCommand(request.Token), ct);
            return NoContent();
        }

        [HttpGet("diagnose")]
        public async Task<IActionResult> Diagnose([FromQuery] Guid? userId, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetNotificationStatusQuery(userId), ct);
            return Ok(result);
        }
    }
}
