using Gatherly.Domain.Entities;
using Gatherly.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Gatherly.Persistence.Repository;

internal sealed class GatheringRepository : IGatheringRepository
{
    private readonly ApplicationDbContext _dbContext;

    public GatheringRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Gathering>> GetByCreatorIdAsync(
        Guid creatorId, 
        CancellationToken cancellationToken = default)
    {
        List<Gathering> gatherings = await _dbContext
            .Set<Gathering>()
            .Where(gathering => gathering.Creator.Id == creatorId)
            .ToListAsync(cancellationToken);

        return gatherings;
    }

    public async Task<Gathering?> GetByIdAsync(Guid id,  CancellationToken cancellationToken = default)
    {
        Gathering? gathering = await _dbContext
            .Set<Gathering>()
            .AsSingleQuery()
            .Include(creator => creator.Creator)
            .Include(attendes => attendes.Attendees)
            .Include(invitations => invitations.Invitations)
            .IgnoreQueryFilters()
            .Where(gathering => gathering.Canceled)
            .FirstOrDefaultAsync(
                gathering => gathering.Id == id,
                cancellationToken);

        return gathering;
    }

    public async Task<Gathering?> GetByIdWithInvitationsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Gathering>()
            .Include(i => i.Invitations)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<Gathering?> GetByIdWithCreatorAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Gathering>()
            .Include(i => i.Creator)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public void Add(Gathering gathering)
    {
        _dbContext.Set<Gathering>().Add(gathering);
    }

    public void Remove(Gathering gathering)
    {
        _dbContext.Set<Gathering>().Remove(gathering);
    }
}