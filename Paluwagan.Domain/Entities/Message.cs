using System;
using System.Diagnostics.CodeAnalysis;
using Paluwagan.Domain.ValueObjects;
using Paluwagan.SharedKernel;
using Paluwagan.SharedKernel.Exceptions;

namespace Paluwagan.Domain.Entities
{
    public sealed class Message : AggregateRoot
    {
        public MessageId Id { get; private set; } = default!;
        public GroupId GroupId { get; private set; } = default!;
        public Guid SenderId { get; private set; }
        public Guid ReceiverId { get; private set; }
        public string? Text { get; private set; }
        public string? ImageUrl { get; private set; }
        public DateTime SentAt { get; private set; }
        public bool IsRead { get; private set; }

        protected Message() { }

        [SetsRequiredMembers]
        private Message(GroupId groupId, Guid senderId, Guid receiverId, string? text, string? imageUrl)
        {
            Id = new MessageId(Guid.NewGuid());
            GroupId = groupId;
            SenderId = senderId;
            ReceiverId = receiverId;
            Text = text;
            ImageUrl = imageUrl;
            SentAt = DateTime.UtcNow;
            IsRead = false;
        }

        public static Message Create(GroupId groupId, Guid senderId, Guid receiverId, string? text, string? imageUrl)
        {
            if (groupId is null || groupId.Value == Guid.Empty)
                throw new BusinessRuleBrokenException("Group ID is required.");

            if (senderId == Guid.Empty)
                throw new BusinessRuleBrokenException("Sender ID is required.");

            if (receiverId == Guid.Empty)
                throw new BusinessRuleBrokenException("Receiver ID is required.");

            if (senderId == receiverId)
                throw new BusinessRuleBrokenException("Sender and receiver cannot be the same user.");

            if (string.IsNullOrWhiteSpace(text) && string.IsNullOrWhiteSpace(imageUrl))
                throw new BusinessRuleBrokenException("Message must contain text or an image.");

            return new Message(groupId, senderId, receiverId, text, imageUrl);
        }
    }
}
