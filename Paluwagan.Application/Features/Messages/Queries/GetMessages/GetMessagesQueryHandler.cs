using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Mappings;
using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain;
using Paluwagan.Domain.Services;
using Paluwagan.SharedKernel.Exceptions;

namespace Paluwagan.Application.Features.Messages.Queries.GetMessages
{
    internal sealed class GetMessagesQueryHandler(IUnitOfWork unitOfWork, IUserContextService userContext)
        : IQueryHandler<GetMessagesQuery, IReadOnlyList<MessageResponse>>
    {
        public async Task<IReadOnlyList<MessageResponse>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = userContext.GetCurrentUserId();

            var currentUser = await unitOfWork.UserRepository
                .GetByIdAsync(currentUserId)
                .ConfigureAwait(false)
                ?? throw new NotFoundException($"User {currentUserId} was not found.");

            var otherUser = await unitOfWork.UserRepository
                .GetByIdAsync(request.OtherUserId)
                .ConfigureAwait(false)
                ?? throw new NotFoundException($"User {request.OtherUserId} was not found.");

            var messages = await unitOfWork.MessageRepository
                .GetConversationAsync(request.GroupId, currentUserId, request.OtherUserId, cancellationToken)
                .ConfigureAwait(false);

            var nameMap = new Dictionary<Guid, string>
            {
                [currentUserId] = currentUser.FullName,
                [request.OtherUserId] = otherUser.FullName
            };

            return messages
                .Select(m => m.ToMessageResponse(nameMap.TryGetValue(m.SenderId, out var name) ? name : string.Empty))
                .ToList();
        }
    }
}
