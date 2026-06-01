using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Paluwagan.API.Dtos;
using Paluwagan.Application.Features.Profile.Commands;
using Paluwagan.Application.Features.Profile.Queries;

namespace Paluwagan.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfilesController : Controller
    {
        private readonly IMediator _mediator;

        public ProfilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetProfileQuery(), ct);
            return Ok(result);
        }

        [HttpPut("payment")]
        public async Task<IActionResult> UpdatePaymentDetails([FromBody] UpdatePaymentDetailsRequest request, CancellationToken ct)
        {
            var result = await _mediator.Send(
                new UpdatePaymentDetailsCommand(request.GCashNumber, request.MayaNumber), ct);
            return Ok(result);
        }
    }
}
