using Dashboard.Models;

namespace Dashboard.Services;

public class StatisticsService(TradeService tradeService)
{
    public async Task<IEnumerable<Statistics>> GetAll()
    {
        var query = await tradeService.GetAll(new QueryParams());
        var result = query.GroupBy(x => x.Ticker).Select(x =>
            {
                var sumCount = x.Where(t => t.BuySell == BuySell.B).Sum(t => t.Count) -
                               x.Where(t => t.BuySell == BuySell.S).Sum(t => t.Count);
                var total = x.Where(t => t.BuySell == BuySell.B).Sum(t => t.Sum) -
                            x.Where(t => t.BuySell == BuySell.S).Sum(t => t.Sum);
                if (sumCount == 0) return null;
                return new Statistics
                {
                    Id = 0,
                    Ticker = x.Key,
                    Currency = x.First().Currency,
                    SumCount = sumCount,
                    Total = total,
                    AvgPrice = Math.Round(total / sumCount, 2, MidpointRounding.AwayFromZero),
                };
            })
            .Where(x => x != null)
            .OrderByDescending(x => x!.Total)
            .Select((x, i) =>
            {
                x.Id = i + 1;
                return x;
            });
        return result.ToList();
    }
}