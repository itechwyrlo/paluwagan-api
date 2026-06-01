using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Paluwagan.Application.DTOs;
using Paluwagan.Application.Messaging.Abstractions;
using Paluwagan.Domain;
using Paluwagan.Domain.Services;
using Paluwagan.Domain.ValueObjects;
using Paluwagan.SharedKernel.Exceptions;

namespace Paluwagan.Application.Features.Groups.Commands
{
    internal sealed class MarkPaymentAsPaidCommandHandler(IUnitOfWork unitOfWork, IUserContextService userContext)
        : ICommandHandler<MarkPaymentAsPaidCommand, MarkPaymentAsPaidResponse>
    {
        public async Task<MarkPaymentAsPaidResponse> Handle(MarkPaymentAsPaidCommand command, CancellationToken cancellationToken)
        {
            var organizerId = userContext.GetCurrentUserId();

            var group = await unitOfWork.GroupRepository
                .GetByIdWithDetailsAsync(new GroupId(command.GroupId))
                .ConfigureAwait(false)
                ?? throw new NotFoundException($"Group {command.GroupId} was not found.");

            group.MarkPaymentAsPaid(organizerId, command.MemberId, command.Round);

            await unitOfWork.CompleteAsync(cancellationToken).ConfigureAwait(false);

            var payment = group.Payments.First(p => p.MemberId == command.MemberId && p.Round == command.Round);

            return new MarkPaymentAsPaidResponse(payment.MemberId, payment.Round, payment.IsPaid, payment.PaidAt);
        }
    }
}
