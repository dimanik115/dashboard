namespace Dashboard.Models;

public class Statistics
{
    /// <summary>Id</summary>
    public required int Id { get; set; }

    /// <summary>Тикер</summary>
    public required string Ticker { get; set; }

    /// <summary>Средняя цена в целом</summary>
    public required decimal AvgPrice { get; set; }

    /// <summary>Сумма по количеству штук</summary>
    public required int SumCount { get; set; }

    /// <summary>Сумма по объему денег</summary>
    public required decimal Total { get; set; }

    /// <summary>Валюта</summary>
    public required Currency Currency { get; set; }
}