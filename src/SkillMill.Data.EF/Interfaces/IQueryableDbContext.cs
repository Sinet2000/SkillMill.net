using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SkillMill.Domain;

namespace SkillMill.Data.EF.Interfaces;

public interface IQueryableDbContext<TContext>
    where TContext : DbContext
{
    TContext Context { get; init; }

    IQueryable<T> List<T>(bool asTracking = true, IEnumerable<string>? toInclude = null, Expression<Func<T, bool>>? filterBy = null) where T : class, IBaseEntity;

    Task<T?> GetByIdAsync<T>(int id, bool asTracking = true) where T : class, IBaseEntity;

    Task<T?> GetSingleByIdAsync<T>(int id, bool asTracking = true) where T : class, IBaseEntity;
}