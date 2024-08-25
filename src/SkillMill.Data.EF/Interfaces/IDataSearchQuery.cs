using System.Linq.Expressions;
using Gridify;
using Sieve.Models;
using SkillMill.Data.Common.Models;
using SkillMill.Domain;

namespace SkillMill.Data.EF.Interfaces;

public interface IDataSearchQuery
{
    Task<SievePagedResult<TReturn>> ListAsync<T, TReturn>(
        SieveModel request,
        IEnumerable<string>? toInclude = null,
        Expression<Func<T, bool>>? filterBy = null,
        CancellationToken cancellationToken = default)
        where T : class, IBaseEntity
        where TReturn : class;

    Task<GridifyPagedResult<TReturn>> ListAsync<T, TReturn>(
        IGridifyQuery query,
        IEnumerable<string>? toInclude = null,
        Expression<Func<T, bool>>? filterBy = null,
        CancellationToken cancellationToken = default)
        where T : class, IBaseEntity
        where TReturn : class;
}