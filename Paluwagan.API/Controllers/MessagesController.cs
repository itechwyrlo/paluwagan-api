using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Paluwagan.API.Dtos;
using Paluwagan.Application.Features.Messages.Commands.SendMessage;
using Paluwagan.Application.Features.Messages.Commands.UploadChatImage;
using Paluwagan.Application.Features.Messages.Queries.GetMessages;
using Paluwagan.Infrastructure.Configurations;

namespace Paluwagan.API.Controllers
{
    [ApiController]
    [Route("api/messages")]
    [Authorize(Policy = Policies.OrganizerOrMember)]
    public class MessagesController : Controller
    {
        private readonly IMediator _mediator;

        public MessagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(
            [FromQuery] Guid groupId,
            [FromQuery] Guid otherUserId,
            CancellationToken ct)
        {
            var result = await _mediator.Send(new GetMessagesQuery(groupId, otherUserId), ct);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request, CancellationToken ct)
        {
            var result = await _mediator.Send(
                new SendMessageCommand(request.GroupId, request.ReceiverId, request.Text, request.ImageUrl), ct);
            return Ok(result);
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, CancellationToken ct)
        {
            if (file is null)
                return BadRequest(new { error = "No file provided." });

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms, ct);

            var imageUrl = await _mediator.Send(
                new UploadChatImageCommand(ms.ToArray(), file.FileName, file.ContentType), ct);

            return Ok(new { imageUrl });
        }
    }
}
