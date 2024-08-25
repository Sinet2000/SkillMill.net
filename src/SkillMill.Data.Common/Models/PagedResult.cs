using Sieve.Models;

namespace SkillMill.Data.Common.Models;

public class PagedResult<T>
    where T : class
{
    public PagedResult(SieveModel request, int totalCount)
    {
        request.PageSize ??= DataConst.DefaultPageSize;

        PageSize = request.PageSize.Value;
        OrderBy = request.Sorts;
        TotalCount = totalCount;
        TotalPagesCount = (int)Math.Ceiling((double)TotalCount / PageSize);
    }

    public int PageSize { get; private set; }

    public string OrderBy { get; private set; }

    public int TotalCount { get; private set; }

    public int TotalPagesCount { get; private set; }

    public int Count => Data.Count;

    public IList<T> Data { get; init; } = new List<T>();
}