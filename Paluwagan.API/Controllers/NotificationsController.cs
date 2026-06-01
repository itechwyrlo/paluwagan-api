using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Paluwagan.API.Dtos;
using Paluwagan.Application.Features.Notifications.Commands.SaveFcmToken;
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
    }
}
