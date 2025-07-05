using MiniExcelLibs.Attributes;

namespace Dashboard.Models;

public class Trade
{
    /// <summary>Id</summary>
    [ExcelIgnore]
    public int Id { get; set; }

    /// <summary>Тикер</summary>
    public string Ticker { get; set; } = null!;

    /// <summary>Дата сделки</summary>
    // [ExcelFormat("o")]
    public DateTime TradeDate { get; set; }

    /// <summary>Средняя цена</summary>
    [ExcelFormat("N2")]
    public decimal AvgPrice { get; set; }

    /// <summary>Кол-во штук</summary>
    public int Count { get; set; }

    /// <summary>Сумма по позиции</summary>
    [ExcelFormat("N2")]
    public decimal Sum { get; set; }

    /// <summary>Валюта</summary>
    public Currency Currency { get; set; }

    /// <summary>Покупка или продажа</summary>
    public BuySell BuySell { get; set; }

    /// <summary>Брокер</summary>
    public Broker Broker { get; set; }
}


public class Seed
{
    /// <summary>Сколько вложено</summary>
    [ExcelColumnIndex("B")]
    public int Money { get; set; }
}