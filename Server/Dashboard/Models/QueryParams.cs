namespace Dashboard.Models;

public record QueryParams
{
    /// <summary> Сортировка </summary>
    public IEnumerable<Sort>? Sort { get; init; }

    /// <summary> Фильтр </summary>
    public Filter? Filter { get; init; }

    /// <summary>Номер страницы</summary>
    public int? Page { get; init; }

    /// <summary>Число записей на странице</summary>
    public int? PageSize { get; init; }
}