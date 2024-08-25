using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using SkillMill.Data.Common.Models;
using SkillMill.Domain;

namespace SkillMill.Data.EF.Interfaces;

public interface IDataSearchQuery<TContext>
    where TContext : DbContext
{
    Task<PagedResult<TReturn>> ListAsync<T, TReturn>(
        SieveModel request,
        IEnumerable<string>? toInclude = null,
        Expression<Func<T, bool>>? filterBy = null,
        CancellationToken cancellationToken = default
        )
        where T : class, IBaseEntity
        where TReturn : class;
}