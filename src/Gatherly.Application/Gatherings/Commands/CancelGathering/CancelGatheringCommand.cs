using Gatherly.Application.Abstractions.Messaging;

namespace Gatherly.Application.Gatherings.Commands.CancelGathering;

public sealed record CancelGatheringCommand(Guid GatheringId) : ICommand;
