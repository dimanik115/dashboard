namespace Dashboard.Models;

public class TypeStatistics
{
    /// <summary>Id</summary>
    public required int Id { get; set; }

    /// <summary>Тип активов</summary>
    public required string Type { get; set; }

    /// <summary>Сумма по объему денег в рублях</summary>
    public required decimal SumRub { get; set; }

    /// <summary>Процент от всей суммы</summary>
    public required decimal Percent { get; set; }

    /// <summary>Сумма по объему денег в рублях на сейчас</summary>
    public decimal SumRubNow { get; set; }
}