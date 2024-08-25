using Gridify;
using SkillMill.Common;

namespace SkillMill.Data.Common.Models;

public class GridifyPagedResult<T>
    where T : class
{
    public GridifyPagedResult(IGridifyQuery query, int totalCount)
    {
        query.PageSize = query.PageSize.HasValue() ? query.PageSize : DataConst.DefaultPageSize;
        PageSize = query.PageSize;
        OrderBy = query.OrderBy ?? string.Empty;
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