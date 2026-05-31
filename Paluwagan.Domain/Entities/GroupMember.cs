using System;
using System.Diagnostics.CodeAnalysis;
using Paluwagan.Domain.ValueObjects;
using Paluwagan.SharedKernel.Exceptions;

namespace Paluwagan.Domain.Entities
{
    public sealed class GroupMember
    {
        public GroupMemberId Id { get; private set; } = default!;
        public GroupId GroupId { get; private set; } = default!;
        public Guid UserId { get; private set; }
        public DateTime JoinedAt { get; private set; }
        public int SlotNumber { get; private set; }
        public int PayoutOrder { get; private set; }

        protected GroupMember() { }

        [SetsRequiredMembers]
        internal GroupMember(GroupId groupId, Guid userId, int slotNumber, int payoutOrder)
        {
            if (groupId is null || groupId.Value == Guid.Empty)
                throw new BusinessRuleBrokenException("Group ID is required.");

            if (userId == Guid.Empty)
                throw new BusinessRuleBrokenException("User ID is required.");

            Id = new GroupMemberId(Guid.NewGuid());
            GroupId = groupId;
            UserId = userId;
            JoinedAt = DateTime.UtcNow;
            SlotNumber = slotNumber;
            PayoutOrder = payoutOrder;
        }
    }
}
