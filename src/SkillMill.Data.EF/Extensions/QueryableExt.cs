using Microsoft.EntityFrameworkCore;

namespace SkillMill.Data.EF.Extensions;

public static class QueryableExt
{
    public static IQueryable<T> WithIncludes<T>(this IQueryable<T> query, IEnumerable<string>? includes = null)
        where T : class
    {
        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        return query;
    }
}