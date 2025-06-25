using System.Text.Json.Serialization;
using MiniExcelLibs.Attributes;

namespace Dashboard.Models;

public class Trade
{
    public string Ticker { get; set; } = null!;

    // [ExcelFormat("o")]
    public DateTime TradeDate { get; set; }

    [ExcelFormat("N2")]
    public decimal AvgPrice { get; set; }

    public int Count { get; set; }

    [ExcelFormat("N2")]
    public decimal Sum { get; set; }

    public Currency Currency { get; set; }
    public BuySell BuySell { get; set; }
    public Broker Broker { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Broker
{
    Alpha,
    Bcs,
    T,
    Sber,
    Finam,
    Vtb,
    Cifra,
    Mkb,
    Gpb,
    Mts
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BuySell
{
    B,
    S
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Currency
{
    // ReSharper disable InconsistentNaming
    RUB,
    USD,
    EUR
}

public class Seed
{
    [ExcelColumnIndex("B")]
    public int Money { get; set; }
}