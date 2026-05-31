using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Paluwagan.API.Dtos;
using Paluwagan.Application.Features.Groups.Commands;
using Paluwagan.Application.Features.Groups.Queries;
using Paluwagan.Application.Features.Groups.Queries.GetAll;
using Paluwagan.Application.Features.Groups.Queries.GetById;
using Paluwagan.Application.Features.Groups.Queries.GetPayments;
using Paluwagan.Application.Features.Groups.Queries.GetHistory;
using Paluwagan.Application.Features.Members.Commands;
using Paluwagan.Domain.Services;
using Paluwagan.Infrastructure.Configurations;
using Paluwagan.SharedKernel.Models;
using Paluwagan.Application.Features.Groups.Queries.GetMembers;

namespace Paluwagan.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<GroupsController> _logger;

        public GroupsController(IMediator mediator, ILogger<GroupsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = Policies.OrganizerOrMember)]
        public async Task<IActionResult> GetAllGroups([FromQuery] QueryObjectParams? param, CancellationToken ct)
        {
    
            var result = await _mediator.Send(new GroupQueryAll(param), ct);
            return Ok(result);
        }

        [HttpPost]
        [Authorize()]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest request, CancellationToken ct)
        {
            var result = await _mediator.Send(new CreateGroupCommand(
                request.Name,
                request.ContributionAmount,
                request.Schedule,
                request.NumberOfSlots,
                request.StartDate), ct);

            return CreatedAtAction(nameof(CreateGroup), new { id = result.GroupId }, result);
        }

        [HttpGet("{groupId:guid}")]
        [Authorize(Policy = Policies.OrganizerOrMember)]
        public async Task<IActionResult> GetGroupById(Guid groupId, CancellationToken ct)
        {
            var result = await _mediator.Send(new GroupQueryGetById(groupId), ct);
            return Ok(result);
        }

        [HttpGet("{groupId:guid}/members")]
        [Authorize(Policy = Policies.OrganizerOrMember)]
        public async Task<IActionResult> GetGroupMembers(Guid groupId, CancellationToken ct)
        {
            var result = await _mediator.Send(new GroupQueryGetMembers(groupId), ct);
            return Ok(result);
        }

        [HttpGet("{groupId:guid}/payments")]
        [Authorize(Policy = Policies.OrganizerOrMember)]
        public async Task<IActionResult> GetGroupPayments(Guid groupId, CancellationToken ct)
        {
            var result = await _mediator.Send(new GroupQueryGetPayments(groupId), ct);
            return Ok(result);
        }

        [HttpGet("{groupId:guid}/history")]
        [Authorize(Policy = Policies.OrganizerOrMember)]
        public async Task<IActionResult> GetGroupHistory(Guid groupId, CancellationToken ct)
        {
            var result = await _mediator.Send(new GroupQueryGetHistory(groupId), ct);
            return Ok(result);
        }

        [HttpPost("{groupId:guid}/members")]
        [Authorize(Policy = Policies.OrganizerOnly)]
        public async Task<IActionResult> AddMember(Guid groupId, [FromBody] AddMemberRequest request, CancellationToken ct)
        {
            await _mediator.Send(new AddMemberCommand(groupId, request.AccountId, request.SlotNumber), ct);
            return NoContent();
        }
    }
}
