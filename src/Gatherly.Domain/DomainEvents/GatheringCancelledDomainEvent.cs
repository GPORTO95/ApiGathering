namespace Gatherly.Domain.DomainEvents;

public sealed record GatheringCancelledDomainEvent(Guid Id, Guid GatheringId): DomainEvent(Id);
