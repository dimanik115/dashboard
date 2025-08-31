using MiniExcelLibs.Attributes;

namespace Dashboard.Models;

public class Bookmark
{
    [ExcelIgnore]
    public int Id { get; set; }

    /// <summary>Источник данных</summary>
    public string Source { get; set; } = null!;

    /// <summary>Тикер (символ)</summary>
    public string Ticker { get; set; } = null!;

    /// <summary>Поле для группировки</summary>
    public string Group { get; set; } = null!;

    /// <summary>Тип актива</summary>
    public string Type { get; set; } = null!;

    /// <summary>Страна</summary>
    public string Country { get; set; } = null!;

    /// <summary>Сектор (отрасль)</summary>
    public string Sector { get; set; } = null!;

    /// <summary>Куплено ли</summary>
    public bool IsBought { get; set; }

    /// <summary>Описание</summary>
    public string? Description { get; set; }

    /// <summary>Описание</summary>
    public string? TinkUid { get; set; }

    /// <summary>Цвет для фронта</summary>
    public string? Color { get; set; }
}