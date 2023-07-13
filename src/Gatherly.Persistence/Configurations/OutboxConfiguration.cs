﻿using Gatherly.Domain.Entities;
using Gatherly.Persistence.Constants;
using Gatherly.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gatherly.Persistence.Configurations;

internal sealed class OutboxConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable(TableNames.OutboxMessages);

        builder.HasKey(x => x.Id);
    }
}
