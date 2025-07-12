namespace Dashboard.Models;

public class CountryStatistics
{
    /// <summary>Id</summary>
    public required int Id { get; set; }

    /// <summary>Страна</summary>
    public required string Country { get; set; }

    /// <summary>Сумма по объему денег</summary>
    public required decimal Sum { get; set; }

    /// <summary>Валюта</summary>
    public required Currency Currency { get; set; }

    /// <summary>Сумма по объему денег в рублях</summary>
    public required decimal SumRub { get; set; }

    /// <summary>Процент от всей суммы</summary>
    public required decimal Percent { get; set; }

    /// <summary>Сумма по объему денег в рублях на сейчас</summary>
    public decimal SumRubNow { get; set; }
}