namespace Dashboard.Models;

public class Statistics
{
    /// <summary>Id</summary>
    public int Id { get; set; }

    /// <summary>Тикер</summary>
    public string Ticker { get; set; }

    /// <summary>Средняя цена в целом</summary>
    public decimal AvgPrice { get; set; }

    /// <summary>Сумма по количеству штук инструментов</summary>
    public int SumCount { get; set; }

    /// <summary>Сумма по объему денег</summary>
    public decimal Total { get; set; }

    /// <summary>Доля от общего</summary>
    public decimal Percent { get; set; }

    /// <summary>Валюта</summary>
    public Currency Currency { get; set; }

    /// <summary>Страна</summary>
    public string Country { get; set; }

    /// <summary>Сектор (отрасль)</summary>
    public string Sector { get; set; }

    /// <summary>Тип актива</summary>
    public string Type { get; set; }

    /// <summary>Источник данных</summary>
    public string Source { get; set; }

    /// <summary>Цвет для фронта</summary>
    public string? Color { get; set; }

    /// <summary>Сумма по объему денег в рублях на сейчас</summary>
    public decimal SumRubNow { get; set; }
}