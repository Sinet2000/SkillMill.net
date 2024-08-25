using System.Linq.Expressions;
using AutoMapper;
using Gridify;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using SkillMill.Data.Common.Models;
using SkillMill.Domain;

namespace SkillMill.Data.EF.Interfaces;

public class DataSearchableDbContext<TContext>(TContext context, IMapper mapper, ISieveProcessor sieveProcessor)
    : QueryableDbContext<TContext>(context, mapper), IDataSearchQuery
    where TContext : DbContext
{
    public async Task<SievePagedResult<TReturn>> ListAsync<T, TReturn>(
        SieveModel request,
        IEnumerable<string>? toInclude = null,
        Expression<Func<T, bool>>? filterBy = null,
        CancellationToken cancellationToken = default)
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

        return new SievePagedResult<TReturn>(request, totalCount)
        {
            Data = Mapper.Map<List<TReturn>>(data)
        };
    }

    public async Task<GridifyPagedResult<TReturn>> ListAsync<T, TReturn>(
        IGridifyQuery query,
        IEnumerable<string>? toInclude = null,
        Expression<Func<T, bool>>? filterBy = null,
        CancellationToken cancellationToken = default)
        where T : class, IBaseEntity
        where TReturn : class
    {
        var queriedData = List<T>(asTracking: false, toInclude);
        if (filterBy is not null)
        {
            queriedData = queriedData.Where(filterBy);
        }

        queriedData = queriedData.ApplyFiltering(query);
        int totalCount = await queriedData.CountAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(query.OrderBy))
        {
            query.OrderBy = nameof(IBaseEntity.Id);
        }

        queriedData = queriedData.ApplyOrdering(query.OrderBy).ApplyPaging(query);
        queriedData = queriedData.ApplyPaging(query);
        var data = await queriedData.ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

        return new GridifyPagedResult<TReturn>(query, totalCount)
        {
            Data = Mapper.Map<List<TReturn>>(data)
        };
    }
}