using Gatherly.Domain.Entities;
using Gatherly.Domain.Repositories;
using Gatherly.Persistence.Specifications;
using Gatherly.Persistence.Specifications.GatheringSpec;
using Microsoft.EntityFrameworkCore;

namespace Gatherly.Persistence.Repository;

public sealed class GatheringRepository : IGatheringRepository
{
    private readonly ApplicationDbContext _dbContext;

    public GatheringRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private IQueryable<Gathering> ApplySpecification(
        Specification<Gathering> specification)
    {
        return SpecificationEvaluator.GetQuery(
            _dbContext.Set<Gathering>(),
            specification);
    }

    public async Task<List<Gathering>> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default) =>
        await ApplySpecification(new GatheringByNameSpecification(name))
            .ToListAsync();

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

    public async Task<Gathering?> GetByIdAsync(Guid id,  CancellationToken cancellationToken = default) =>
        await ApplySpecification(new GatheringByIdSplitSpecification(id))
            .FirstOrDefaultAsync();

    public async Task<Gathering?> GetByIdWithInvitationsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Gathering>()
            .Include(i => i.Invitations)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<Gathering?> GetByIdWithCreatorAsync(Guid id, CancellationToken cancellationToken = default) =>
        await ApplySpecification(new GatheringByIdWithCreatorSpecification(id))
            .FirstOrDefaultAsync();

    public void Add(Gathering gathering)
    {
        _dbContext.Set<Gathering>().Add(gathering);
    }

    public void Remove(Gathering gathering)
    {
        _dbContext.Set<Gathering>().Remove(gathering);
    }
}