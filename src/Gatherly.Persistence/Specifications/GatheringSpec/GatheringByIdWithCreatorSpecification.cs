﻿using Gatherly.Domain.Entities;

namespace Gatherly.Persistence.Specifications.GatheringSpec;

internal class GatheringByIdWithCreatorSpecification : Specification<Gathering>
{
    public GatheringByIdWithCreatorSpecification(Guid gatheringId)
        : base(gathering => gathering.Id == gatheringId)
    {
        AddInclude(gathering => gathering.Creator);
    }
}
