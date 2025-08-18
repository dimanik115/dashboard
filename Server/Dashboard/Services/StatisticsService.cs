using Dashboard.Models;

namespace Dashboard.Services;

public class StatisticsService(
    TradeService tradeService,
    BookmarkService bookmarkService,
    CurrencyService currencyService,
    RealDataService realDataService
)
{
    /// <summary>Получить статистику</summary>
    public async Task<List<Statistics>> GetAll(bool isOnline = false)
    {
        var bookmarks = bookmarkService.GetAll();
        var statTrades = (await tradeService.GetAll(new QueryParams())).GroupBy(x => x.Ticker).Select(g =>
            {
                var bookmark = bookmarks.FirstOrDefault(b => b.Ticker == g.Key);

                var sumCount = g.Where(t => t.BuySell == BuySell.Buy).Sum(t => t.Count) -
                               g.Where(t => t.BuySell == BuySell.Sell).Sum(t => t.Count);
                var total = g.Where(t => t.BuySell == BuySell.Buy).Sum(t => t.Sum) -
                            g.Where(t => t.BuySell == BuySell.Sell).Sum(t => t.Sum);
                if (sumCount > 0)
                    return new { Ticker = g.Key, sumCount, total, b = bookmark, t = g.AsEnumerable() };
                return null;
            })
            .Where(x => x != null)
            .ToList();

        var currencyMap = await currencyService.GetAvgPrices();
        var totalMoney = statTrades.Sum(x => x!.t.Sum(t => t.Sum * currencyMap[t.Currency].AvgPrice));

        Dictionary<string, decimal> lastPrices = new();
        Dictionary<Currency, decimal> currencyRates = new();
        if (isOnline)
        {
            currencyRates = await realDataService.GetLastCurrencyRates();
            lastPrices = await realDataService.GetLastPrices(statTrades.Select(x => x!.b.TinkUid).ToList());
        }

        var res = from grouping in statTrades
            let cur = grouping.t.First().Currency
            let avgPrice = Math.Round(grouping.total / grouping.sumCount, 2, MidpointRounding.AwayFromZero)
            orderby grouping.total * currencyMap[cur].AvgPrice descending
            select new Statistics
            {
                Id = 0,
                Ticker = grouping.Ticker,
                Currency = cur,
                SumCount = grouping.sumCount,
                Total = grouping.total,
                Percent = Math.Round(
                    grouping.total * currencyMap[cur].AvgPrice / totalMoney * 100, 2,
                    MidpointRounding.AwayFromZero),
                AvgPrice = avgPrice,
                Country = grouping.b.Country,
                Sector = grouping.b.Sector,
                Type = grouping.b.Type,
                Source = grouping.b.Source,
                SumRubNow = isOnline
                    ? grouping.sumCount * lastPrices.GetValueOrDefault(grouping.b.TinkUid ?? "", avgPrice) * currencyRates[cur]
                    : grouping.total * currencyMap[cur].AvgPrice
            };

        return res.Select((x, i) =>
        {
            x.Id = i + 1;
            return x;
        }).ToList();
    }

    /// <summary>Получить статистику по странам</summary>
    public async Task<IEnumerable<CountryStatistics>> GetAllByCountry(bool isOnline = false)
    {
        var currencyMap = await currencyService.GetAvgPrices();
        var statistics = await GetAll(isOnline);
        var total = statistics.Sum(x => x.Total);
        var bookmarks = bookmarkService.GetAll();
        var res = from stat in statistics
            group stat by bookmarks.FirstOrDefault(book => book.Ticker == stat.Ticker).Country
            into elems
            let cur = elems.First().Currency
            let sumRub = elems.Sum(x => x.Total) * currencyMap[cur].AvgPrice
            select new CountryStatistics
            {
                Id = 0,
                Country = elems.Key,
                Currency = cur,
                Sum = elems.Sum(x => x.Total),
                SumRub = sumRub,
                Percent = Math.Round(100 * sumRub / total, 2, MidpointRounding.AwayFromZero),
                SumRubNow = isOnline ? elems.Sum(x => x.SumRubNow) : sumRub
            };

        return res.Select((x, i) =>
        {
            x.Id = i + 1;
            return x;
        }).ToList();
    }

    /// <summary>Получить статистику по секторам</summary>
    public async Task<IEnumerable<SectorStatistics>> GetAllBySector(bool isOnline = false)
    {
        var currencyMap = await currencyService.GetAvgPrices();
        var statistics = await GetAll(isOnline);
        var total = statistics.Sum(x => x.Total);
        var bookmarks = bookmarkService.GetAll();
        var res = from stat in statistics
            group stat by bookmarks.FirstOrDefault(book => book.Ticker == stat.Ticker).Sector
            into elems
            let cur = elems.First().Currency
            let sumRub = elems.Sum(x => x.Total) * currencyMap[cur].AvgPrice
            select new SectorStatistics
            {
                Id = 0,
                Sector = elems.Key,
                SumRub = sumRub,
                Percent = Math.Round(100 * sumRub / total, 2, MidpointRounding.AwayFromZero),
                SumRubNow = isOnline ? elems.Sum(x => x.SumRubNow) : sumRub
            };
        return res.Select((x, i) =>
        {
            x.Id = i + 1;
            return x;
        }).ToList();
    }

    /// <summary>Получить статистику по типу активов</summary>
    public async Task<IEnumerable<TypeStatistics>> GetAllByType(bool isOnline = false)
    {
        var currencyMap = await currencyService.GetAvgPrices();
        var statistics = await GetAll(isOnline);
        var total = statistics.Sum(x => x.Total);
        var bookmarks = bookmarkService.GetAll();
        var res = from stat in statistics
            group stat by bookmarks.FirstOrDefault(book => book.Ticker == stat.Ticker).Type
            into elems
            let cur = elems.First().Currency
            let sumRub = elems.Sum(x => x.Total) * currencyMap[cur].AvgPrice
            select new TypeStatistics
            {
                Id = 0,
                Type = elems.Key,
                SumRub = sumRub,
                Percent = Math.Round(100 * sumRub / total, 2, MidpointRounding.AwayFromZero),
                SumRubNow = isOnline ? elems.Sum(x => x.SumRubNow) : sumRub
            };
        return res.Select((x, i) =>
        {
            x.Id = i + 1;
            return x;
        }).ToList();
    }

    /// <summary>Получить количество денег по всем активам</summary>
    public async Task<decimal> GetTotalMoney(bool isOnline = false)
    {
        var currencyMap = await currencyService.GetAvgPrices();
        var statistics = await GetAll(isOnline);
        var sumRub = isOnline
            ? statistics.Sum(x => x.SumRubNow)
            : statistics.Sum(x => x.Total * currencyMap[x.Currency].AvgPrice);
        return Math.Round(sumRub, 2, MidpointRounding.AwayFromZero);
    }
}