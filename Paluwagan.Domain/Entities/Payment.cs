using System;
using Paluwagan.Domain.ValueObjects;
using Paluwagan.SharedKernel.Exceptions;

namespace Paluwagan.Domain.Entities
{
    public sealed class Payment
    {
        public PaymentId Id { get; private set; } = default!;
        public GroupId GroupId { get; private set; } = default!;
        public Guid MemberId { get; private set; }
        public int Round { get; private set; }
        public bool IsPaid { get; private set; }
        public DateTime? PaidAt { get; private set; }

        protected Payment() { }

        internal Payment(GroupId groupId, Guid memberId, int round)
        {
            if (groupId is null || groupId.Value == Guid.Empty)
                throw new BusinessRuleBrokenException("Group ID is required.");

            if (memberId == Guid.Empty)
                throw new BusinessRuleBrokenException("Member ID is required.");

            if (round < 1)
                throw new BusinessRuleBrokenException("Round must be at least 1.");

            Id = new PaymentId(Guid.NewGuid());
            GroupId = groupId;
            MemberId = memberId;
            Round = round;
            IsPaid = false;
        }

        internal void MarkAsPaid()
        {
            if (IsPaid)
                throw new BusinessRuleBrokenException("Payment has already been marked as paid.");

            IsPaid = true;
            PaidAt = DateTime.UtcNow;
        }
    }
}
