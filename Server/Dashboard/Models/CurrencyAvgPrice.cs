namespace Dashboard.Models;

public class CurrencyAvgPrice
{
    /// <summary>Валюта</summary>
    public Currency Currency { get; set; }

    /// <summary>Средний курс покупки</summary>
    public decimal AvgPrice { get; set; }

    /// <summary>Средний курс покупки на сейчас</summary>
    public decimal AvgPriceNow { get; set; }
}