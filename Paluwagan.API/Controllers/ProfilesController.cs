using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [HttpPost("qr-code")]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<IActionResult> UploadQrCode(IFormFile file, CancellationToken ct)
        {
            if (file is null || file.Length == 0)
                return BadRequest("No file provided.");

            using var ms = new System.IO.MemoryStream();
            await file.CopyToAsync(ms, ct);

            var result = await _mediator.Send(
                new UploadQrCodeCommand(ms.ToArray(), file.FileName, file.ContentType), ct);

            return Ok(result);
        }
    }
}
