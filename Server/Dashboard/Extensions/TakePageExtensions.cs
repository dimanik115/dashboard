using Dashboard.Models;

namespace Dashboard.Extensions;

public static class TakePageExtensions
{
    public static IEnumerable<T> ExTake<T>(this IEnumerable<T> source, int? page, int? pageSize)
    {
        if (page is null or 0 || pageSize is null or 0) return source;
        return source.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
    }

    public static IEnumerable<T> ExTake<T>(this IOrderedEnumerable<T> source, int? page, int? pageSize)
    {
        if (page is null or 0 || pageSize is null or 0) return source;
        return source.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
    }

    public static IEnumerable<T> WithParams<T>(this IEnumerable<T> source, QueryParams queryParams)
    {
        return source
            .ExWhere(queryParams.Filter)
            .ExOrderBy(queryParams.Sort)
            .ExTake(queryParams.Page, queryParams.PageSize);
    }
}