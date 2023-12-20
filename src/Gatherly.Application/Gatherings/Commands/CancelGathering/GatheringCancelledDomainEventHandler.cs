using Gatherly.Application.Abstractions;
using Gatherly.Application.Abstractions.Messaging;
using Gatherly.Domain.DomainEvents;
using Gatherly.Domain.Entities;
using Gatherly.Domain.Repositories;

namespace Gatherly.Application.Gatherings.Commands.CancelGathering;

internal sealed class GatheringCancelledDomainEventHandler
    : IDomainEventHandler<GatheringCancelledDomainEvent>
{
    private readonly IGatheringRepository _gatheringRepository;
    private readonly IEmailService _emailService;

    public GatheringCancelledDomainEventHandler(IGatheringRepository gatheringRepository, IEmailService emailService)
    {
        _gatheringRepository = gatheringRepository;
        _emailService = emailService;
    }

    public async Task Handle(GatheringCancelledDomainEvent notification, CancellationToken cancellationToken)
    {
        Gathering? gathering = await _gatheringRepository.GetByIdAsync(
            notification.GatheringId,
            cancellationToken);

        if (gathering is null)
        {
            return;
        }

        foreach (Attendee attendee in gathering.Attendees)
        {
            await _emailService.SendGatheringCancelledEmailAsync(attendee, cancellationToken);
        }
    }
}
