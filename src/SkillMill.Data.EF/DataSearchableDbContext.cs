using System.Linq.Expressions;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using SkillMill.Data.Common.Models;
using SkillMill.Data.EF.Interfaces;
using SkillMill.Domain;

namespace SkillMill.Data.EF;

public class DataSearchableDbContext<TContext>(TContext context, IMapper mapper, ISieveProcessor sieveProcessor)
    : QueryableDbContext<TContext>(context, mapper), IDataSearchQuery<TContext>
    where TContext : DbContext
{
    public async Task<PagedResult<TReturn>> ListAsync<T, TReturn>(
        SieveModel request,
        IEnumerable<string>? toInclude = null,
        Expression<Func<T, bool>>? filterBy = null,
        CancellationToken cancellationToken = default
        )
        where T : class, IBaseEntity
        where TReturn : class
    {
        var queriedData = List<T>(asTracking: false, toInclude);
        if (filterBy is not null)
        {
            queriedData = queriedData.Where(filterBy);
        }

        queriedData = sieveProcessor.Apply(request, queriedData, applyPagination: false, applyFiltering: true, applySorting: false);
        int totalCount = await queriedData.CountAsync(cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(request.Sorts))
        {
            request.Sorts = nameof(IBaseEntity.Id);
        }

        queriedData = sieveProcessor.Apply(request, queriedData, applySorting: true, applyPagination: true, applyFiltering: false);
        var data = await queriedData.ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

        return new PagedResult<TReturn>(request, totalCount)
        {
            Data = Mapper.Map<List<TReturn>>(data)
        };
    }
}