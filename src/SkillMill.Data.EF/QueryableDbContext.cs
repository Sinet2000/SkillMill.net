using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkillMill.Common.Exceptions;
using SkillMill.Data.EF.Extensions;
using SkillMill.Data.EF.Interfaces;
using SkillMill.Domain;

namespace SkillMill.Data.EF;

public class QueryableDbContext<TContext>(TContext context, IMapper mapper) : IQueryableDbContext<TContext>
    where TContext : DbContext
{
    public TContext Context { get; init; } = context;

    public IMapper Mapper { get; init; } = mapper;

    public IQueryable<T> List<T>(
        bool asTracking = true,
        IEnumerable<string>? toInclude = null,
        Expression<Func<T, bool>>? filterBy = null
        )
        where T : class, IBaseEntity
    {
        var queryableData = asTracking
            ? Context.Set<T>().WithIncludes(toInclude)
            : Context.Set<T>().AsNoTrackingWithIdentityResolution().WithIncludes(toInclude);

        return filterBy != null ? queryableData.Where(filterBy) : queryableData;
    }

    public async Task<T?> GetByIdAsync<T>(int id, bool asTracking = true)
        where T : class, IBaseEntity
    {
        return await List<T>(asTracking).FirstOrDefaultAsync(e => e.Id == id)
            .ConfigureAwait(false);
    }

    public async Task<T?> GetSingleByIdAsync<T>(int id, bool asTracking = true) where T : class, IBaseEntity
    {
        var found = await GetByIdAsync<T>(id, asTracking) ?? throw new NotFoundInDbException(typeof(T), $"{nameof(id)}", id);

        return found;
    }
}