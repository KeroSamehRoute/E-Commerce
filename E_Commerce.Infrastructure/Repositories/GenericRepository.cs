using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Repositories;

internal class GenericRepository<TEntity, TKey>(StoreDbContext dbContext) : IGenericRepository<TEntity, TKey> 
    where TEntity : BaseEntity<TKey>
{
    public void Add(TEntity entity)
    {
        dbContext.Set<TEntity>().Add(entity);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default)
    {
        return await dbContext.Set<TEntity>().ToListAsync(ct);
    }

    public Task<int> CountAsync(ISpecifications<TEntity, TKey> specifications, CancellationToken cancellationToken = default)
    {
        return SpecificationEvaluator.CreateQuery(dbContext.Set<TEntity>(), specifications).CountAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecifications<TEntity, TKey> Spec, CancellationToken ct = default)
    {
        var query = SpecificationEvaluator.CreateQuery(dbContext.Set<TEntity>(), Spec);

        return await query.ToListAsync(ct);
    }

    public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default)
    {
        return await dbContext.Set<TEntity>().FindAsync([id, ct], cancellationToken: ct);
    }

    public async Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, TKey> spec, CancellationToken ct = default)
    {
        var query = SpecificationEvaluator.CreateQuery(dbContext.Set<TEntity>(), spec);

        return await query.FirstOrDefaultAsync(cancellationToken: ct);
    }

    public void Remove(TEntity entity)
    {
        dbContext.Set<TEntity>().Remove(entity);
    }

    public void Update(TEntity entity)
    {
        dbContext.Set<TEntity>().Update(entity);
    }

}
