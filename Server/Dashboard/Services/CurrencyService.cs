using Dashboard.Models;
using MiniExcelLibs;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Dashboard.Services;

public class CurrencyService(IConfiguration configuration, RealDataService realDataService, ILogger<CurrencyService> logger)
{
    private readonly string path = configuration.GetValue<string>("ExcelPath")!;

    /// <summary>Получить все сделки по валютам</summary>
    public IEnumerable<CurrencyTrade> GetAll()
    {
        var result = MiniExcel.Query<CurrencyTrade>(path, "currency").Select((x, i) =>
        {
            x.Id = i + 1;
            return x;
        });
        return result.ToList();
    }

    /// <summary>Получить все средние курсы валют</summary>
    public async Task<Dictionary<Currency, CurrencyAvgPrice>> GetAvgPrices(bool isOnline = false)
    {
        var map = GetAll().GroupBy(x => x.Currency).ToDictionary(x => x.Key,
            g => new CurrencyAvgPrice
            {
                Currency = g.Key,
                AvgPrice = Math.Round(g.Sum(x => x.Sum) / g.Sum(x => x.Count), 2, MidpointRounding.AwayFromZero),
                AvgPriceNow = 1
            }
        );

        map[Currency.RUB] = new() { Currency = Currency.RUB, AvgPrice = 1, AvgPriceNow = 1 };
        if (isOnline)
        {
            try
            {
                var currencyRates = await realDataService.GetLastCurrencyRates();
                foreach (var rate in map)
                {
                    rate.Value.AvgPriceNow = currencyRates[rate.Key];
                }
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "API ERROR");
            }
        }

        return map;
    }
}