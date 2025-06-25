using System.Text.Json.Serialization;

namespace Dashboard.Models;

/// <summary> модель сортировки</summary>
public record Sort(SortDirection Direction, string Field);

/// <summary>Направления сортировки</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortDirection
{
    /// <summary>По возрастанию</summary>
    Asc,

    /// <summary>По убыванию</summary>
    Desc
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TradeSort
{
    TradeDate
}