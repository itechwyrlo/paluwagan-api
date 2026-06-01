using System;
using System.Collections.Generic;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Messaging.Abstractions;

namespace Paluwagan.Application.Features.Messages.Queries.GetMessages
{
    public sealed record GetMessagesQuery(Guid GroupId, Guid OtherUserId) : IQuery<IReadOnlyList<MessageResponse>>;
}
