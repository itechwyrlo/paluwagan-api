using System;
using System.Collections.Generic;
using System.Linq;
using Paluwagan.Application.DTOs;
using Paluwagan.Domain.Entities;

namespace Paluwagan.Application.Mappings
{
    public static class GroupMappings
    {
        public static GroupSummaryResponse ToGroupSummary(this Group group, Guid currentUserId) =>
                new()
                {
                    Id = group.Id.Value.ToString(),
                    Name = group.Name,
                    ContributionAmount = group.ContributionAmount,
                    Schedule = group.Schedule.ToString(),
                    TotalSlots = group.NumberOfSlots,
                    CurrentRound = group.CurrentRound,
                    OrganizerId = group.OrganizerId.ToString(),
                    MyPaymentStatus = group.Payments
                        .Any(p => p.Round == group.CurrentRound && p.IsPaid),
                    PaidCount = group.Payments
                        .Count(p => p.Round == group.CurrentRound && p.IsPaid)
                };

        public static GroupDetailResponse ToGroupDetail(this Group group, ApplicationUser organizer) =>
            new(
                Id: group.Id.Value.ToString(),
                Name: group.Name,
                ContributionAmount: group.ContributionAmount,
                Schedule: group.Schedule.ToString(),
                NumberOfSlots: group.NumberOfSlots,
                CurrentRound: group.CurrentRound,
                OrganizerId: group.OrganizerId.ToString(),
                OrganizerQrCodeUrl: organizer.QrCodeUrl,
                StartDate: group.StartDate,
                Status: group.Status,
                PaidCount: group.Payments.Count(p => p.Round == group.CurrentRound && p.IsPaid),
                CollectedAmount: group.Payments.Count(p => p.IsPaid) * group.ContributionAmount
            );

        public static IReadOnlyList<MemberDetail> ToMemberDetails(
     this Group group,
     IEnumerable<ApplicationUser> users)
        {
            return users
                .Select(u =>
                {
                    var member = group.Members.First(m => m.UserId == u.Id);

                    return new MemberDetail(
                        UserId: u.Id,
                        FullName: u.FullName,
                        SlotNumber: member.SlotNumber,
                        PayoutOrder: member.PayoutOrder,
                        QrCodeUrl: u.QrCodeUrl
                    );
                })
                .ToList();
        }

        public static IReadOnlyList<PaymentStatus> ToPaymentStatuses(
            this Group group, IEnumerable<ApplicationUser> users) =>
            [..group.Payments
                .Where(p => p.Round == group.CurrentRound)
                .Select(p =>
                {
                    var user = users.FirstOrDefault(u => u.Id == p.MemberId);
                    return new PaymentStatus(
                        MemberId: p.MemberId,
                        MemberName: user?.FullName ?? string.Empty,
                        Round: p.Round,
                        IsPaid: p.IsPaid,
                        PaidAt: p.PaidAt
                    );
                })];

        public static IReadOnlyList<PayoutHistory> ToPayoutHistories(
            this Group group, Dictionary<Guid, ApplicationUser> userDict)
        {
            return [..Enumerable.Range(1, group.CurrentRound - 1)
                .Select(round =>
                {
                    var recipient = group.Members.FirstOrDefault(m => m.PayoutOrder == round);
                    if (recipient is null) return null;

                    var lastPaidAt = group.Payments
                        .Where(p => p.Round == round && p.IsPaid && p.PaidAt.HasValue)
                        .Max(p => p.PaidAt);

                    if (!lastPaidAt.HasValue) return null;

                    userDict.TryGetValue(recipient.UserId, out var user);
                    return new PayoutHistory(
                        MemberId: recipient.UserId,
                        MemberName: user?.FullName ?? string.Empty,
                        Round: round,
                        Amount: group.ContributionAmount * group.Members.Count,
                        PaidAt: lastPaidAt.Value
                    );
                })
                .Where(p => p is not null)
                .Select(p => p!)];
        }
    }
}
