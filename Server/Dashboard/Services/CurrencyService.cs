using Dashboard.Models;
using MiniExcelLibs;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Dashboard.Services;

public class CurrencyService(IConfiguration configuration, RealDataService realDataService)
{
    private readonly string path = configuration.GetValue<string>("ExcelPath")!;

    /// <summary>Получить все сделки по валютам</summary>
    public async Task<List<CurrencyTrade>> GetAll()
    {
        var result = MiniExcel.Query<CurrencyTrade>(path, "currency").Select((x, i) =>
        {
            x.Id = i + 1;
            return x;
        });
        return result.ToList();
    }

    /// <summary>Получить все средние курсы валют</summary>
    public async Task<List<CurrencyAvgPrice>> GetAvgPrices(bool isOnline = false)
    {
        var currencyTrades = await GetAll();
        var res = currencyTrades.GroupBy(x => x.Currency)
            .Select(x => new CurrencyAvgPrice
            {
                Currency = x.Key,
                AvgPrice = Math.Round(x.Sum(x => x.Sum) / x.Sum(x => x.Count), 2, MidpointRounding.AwayFromZero)
            });
        if (isOnline)
        {
            var currencyRates = await realDataService.GetLastCurrencyRates();
            res = res.Select(x =>
            {
                x.AvgPriceNow = currencyRates[x.Currency];
                return x;
            });
        }

        return res.ToList();
    }
}