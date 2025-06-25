using System.Text.Json.Serialization;

namespace Dashboard.Models;

/// <summary> Фильтр </summary>
public record Filter(LogicalOperation LogicalOperation, IEnumerable<FilterItem> Items);

public record FilterItem(string Field, object Value, FilterOperation FilterOperation);

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LogicalOperation
{
    And,
    Or
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FilterOperation
{
    Equal,
    NotEqual,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual,
    Contains,
    In
}