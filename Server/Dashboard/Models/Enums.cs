using System.Text.Json.Serialization;

namespace Dashboard.Models;

// ReSharper disable  InconsistentNaming
/// <summary>Брокер</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Broker
{
    ALPHA,
    BCS,
    T,
    SBER,
    FINAM,
    VTB,
    CIFRA,
    MKB,
    GPB,
    MTS
}

/// <summary>Покупка или продажа</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BuySell
{
    Buy,
    Sell
}

/// <summary>Валюта</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Currency
{
    RUB,
    USD,
    EUR,
    HKD,
    CNY,
    AED
}