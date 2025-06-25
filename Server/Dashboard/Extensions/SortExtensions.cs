using Dashboard.Models;

namespace Dashboard.Extensions;

public static class SortExtensions
{
    public static IOrderedEnumerable<T> ExOrderBy<T>(this IEnumerable<T> source, IEnumerable<Sort>? sorts)
    {
        var sortList = sorts?.ToList() ?? [];
        return sortList.Count == 0
            ? source.OrderBy(_ => 0)
            : sortList.Aggregate(source.ExOrderBy(sortList.First()), (current, item) => current.ExThenBy(item));
    }

    /// <summary>Метод для сортировки последовательности по одному выражению сортировки</summary>
    private static IOrderedEnumerable<T> ExOrderBy<T>(this IEnumerable<T> sequence, Sort sort)
    {
        var accessor = SortUtils.GetPropertyAccessor<T>(sort.Field).Compile();
        return sort.Direction switch
        {
            SortDirection.Asc => sequence.OrderBy(accessor),
            SortDirection.Desc => sequence.OrderByDescending(accessor),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    /// <summary>Метод для добавления дополнительной сортировки к уже отсортированной последовательности</summary>
    private static IOrderedEnumerable<T> ExThenBy<T>(this IOrderedEnumerable<T> sequence, Sort sort)
    {
        var accessor = SortUtils.GetPropertyAccessor<T>(sort.Field).Compile();
        return sort.Direction switch
        {
            SortDirection.Asc => sequence.ThenBy(accessor),
            SortDirection.Desc => sequence.ThenByDescending(accessor),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}