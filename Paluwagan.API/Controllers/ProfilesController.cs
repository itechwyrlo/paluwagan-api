using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    }
}
