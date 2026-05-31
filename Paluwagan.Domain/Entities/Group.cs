using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Paluwagan.Domain.Enums;
using Paluwagan.Domain.ValueObjects;
using Paluwagan.SharedKernel;
using Paluwagan.SharedKernel.Exceptions;

namespace Paluwagan.Domain.Entities
{
    public sealed class Group : AggregateRoot
    {
        public GroupId Id { get; private set; } = default!;
        public string Name { get; private set; } = string.Empty;
        public decimal ContributionAmount { get; private set; }
        public PaymentSchedule Schedule { get; private set; }
        public int NumberOfSlots { get; private set; }
        public DateTime StartDate { get; private set; }
        public Guid OrganizerId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public int CurrentRound { get; private set; } = 1;
        public GroupStatus Status { get; private set; } = GroupStatus.Active;

        private readonly List<GroupMember> _members = new List<GroupMember>();
        public IReadOnlyCollection<GroupMember> Members => _members.AsReadOnly();

        private readonly List<Payment> _payments = new List<Payment>();
        public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();

        protected Group() { }

        [SetsRequiredMembers]
        private Group(string name, decimal contributionAmount, PaymentSchedule schedule,
                      int numberOfSlots, DateTime startDate, Guid organizerId)
        {
            Id = new GroupId(Guid.NewGuid());
            Name = name;
            ContributionAmount = contributionAmount;
            Schedule = schedule;
            NumberOfSlots = numberOfSlots;
            StartDate = startDate;
            OrganizerId = organizerId;
            CreatedAt = DateTime.UtcNow;
        }

        public static Group Create(string name, decimal contributionAmount, PaymentSchedule schedule,
                                   int numberOfSlots, DateTime startDate, Guid organizerId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new BusinessRuleBrokenException("Group name is required.");

            if (contributionAmount <= 0)
                throw new BusinessRuleBrokenException("Contribution amount must be greater than zero.");

            if (numberOfSlots < 2)
                throw new BusinessRuleBrokenException("A group must have at least 2 slots.");

            if (startDate < DateTime.UtcNow.Date)
                throw new BusinessRuleBrokenException("Start date cannot be in the past.");

            if (organizerId == Guid.Empty)
                throw new BusinessRuleBrokenException("Organizer ID is required.");

            return new Group(name, contributionAmount, schedule, numberOfSlots, startDate, organizerId);
        }

        public void AddMember(Guid requestingUserId, Guid userId, int slotNumber)
        {
            if (requestingUserId != OrganizerId)
                throw new BusinessRuleBrokenException("Only the organizer can add members.");

            if (userId == Guid.Empty)
                throw new BusinessRuleBrokenException("User ID is required.");

            if (userId == OrganizerId)
                throw new BusinessRuleBrokenException("Organizer cannot be added as a member.");

            if (_members.Count >= NumberOfSlots)
                throw new BusinessRuleBrokenException("Group is already full.");

            if (_members.Any(m => m.UserId == userId))
                throw new BusinessRuleBrokenException("User is already a member of this group.");

            if (slotNumber < 1 || slotNumber > NumberOfSlots)
                throw new BusinessRuleBrokenException("Slot number is out of range.");

            if (_members.Any(m => m.SlotNumber == slotNumber))
                throw new BusinessRuleBrokenException("Slot is already taken.");

            int payoutOrder = _members.Count + 1;
            _members.Add(new GroupMember(Id, userId, slotNumber, payoutOrder));
            _payments.Add(new Payment(Id, userId, CurrentRound));
        }

        public void RemoveMember(Guid userId)
        {
            var member = _members.FirstOrDefault(m => m.UserId == userId)
                ?? throw new BusinessRuleBrokenException("User is not a member of this group.");

            _members.Remove(member);
        }

        public void AdvanceRound()
        {
            if (Status != GroupStatus.Active)
                throw new BusinessRuleBrokenException("Cannot advance round on a non-active group.");

            if (CurrentRound >= NumberOfSlots)
            {
                Status = GroupStatus.Completed;
                return;
            }

            CurrentRound++;
            foreach (var member in _members)
                _payments.Add(new Payment(Id, member.UserId, CurrentRound));
        }

        public void Cancel()
        {
            if (Status == GroupStatus.Completed)
                throw new BusinessRuleBrokenException("Cannot cancel a completed group.");

            Status = GroupStatus.Cancelled;
        }
    }
}
