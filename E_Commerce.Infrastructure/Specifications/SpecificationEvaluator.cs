using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Specifications;

internal static class SpecificationEvaluator
{

    public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> inputQuery, ISpecifications<TEntity, TKey> spec)
        where TEntity : BaseEntity<TKey>
    {
        var query = inputQuery;

        if (spec.Criteria is not null)
        {
            query = query.Where(spec.Criteria);
        }

        if (spec.IncludeExpressions.Count > 0)
        {
            query = spec.IncludeExpressions.Aggregate(query, (current, nextExp) => current.Include(nextExp));
        }

        if (spec.Orderby is not null)
        {
            query = query.OrderBy(spec.Orderby);
        }
        else if (spec.OrderbyDescending is not null)
        {
            query = query.OrderByDescending(spec.OrderbyDescending);
        }

        if (spec.IsPaginated)
        {
            query = query.Skip(spec.Skip).Take(spec.Take);
        }

        return query;
    }

}
