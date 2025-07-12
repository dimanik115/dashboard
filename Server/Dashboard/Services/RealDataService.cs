using System.Text.Json;
using Dashboard.Models;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Currency = Dashboard.Models.Currency;

namespace Dashboard.Services;

public class RealDataService(InvestApiClient apiClient)
{
    private static readonly SemaphoreSlim CurrencySemaphore = new(1, 1);
    private static readonly SemaphoreSlim Semaphore = new(1, 1);
    private static Dictionary<Currency, decimal> _currencyRates = new();
    private static Dictionary<string, decimal> _lastPrices = new();

    ///<seealso href="https://www.cbr-xml-daily.ru/"/>
    public async Task<Dictionary<Currency, decimal>> GetLastCurrencyRates()
    {
        await CurrencySemaphore.WaitAsync();
        if (!_currencyRates.Any())
        {
            var json = await new HttpClient().GetFromJsonAsync<JsonElement>("https://www.cbr-xml-daily.ru/daily_json.js");

            // foreach (var currency in Enum.GetValues<Currency>().ToDictionary(x => x, x => 1m))
            // {
            //     var val = json.GetProperty("Valute");
            //     if (val.TryGetProperty(currency.Key.ToString(), out var valVal))
            //     {
            //         _currencyRates[currency.Key] =
            //             valVal.GetProperty("Value").GetDecimal() / valVal.GetProperty("Nominal").GetInt32();
            //     }
            // }

            _currencyRates = Enum.GetValues<Currency>().ToDictionary(x => x,
                x =>
                {
                    var valute = json.GetProperty("Valute");
                    if (valute.TryGetProperty(x.ToString(), out var val))
                    {
                        return val.GetProperty("Value").GetDecimal() / val.GetProperty("Nominal").GetInt32();
                    }

                    return 1m;
                });
        }

        CurrencySemaphore.Release();
        return _currencyRates;
    }

    ///<summary>
    ///<see href="https://tinkoff.github.io/investAPI/">tink api (не все)</see>
    ///<see href="https://tinkoff.github.io/investAPI/swagger-ui/#/">tink swagger</see>
    ///<see href="https://twelvedata.com/pricing"> us only</see>
    ///<see href="https://www.tradingview.com/data-coverage/">tradingview остальное</see>
    /// </summary>
    public async Task<Dictionary<string, decimal>> GetLastPrices(IEnumerable<string> uids)
    {
        await Semaphore.WaitAsync();
        try
        {
            if (!_lastPrices.Any())
            {
                var res = apiClient.MarketData.GetLastPrices(new GetLastPricesRequest
                {
                    InstrumentId = { uids.Where(x => x != null) }
                }).LastPrices;

                _lastPrices = res.Where(x => !string.IsNullOrEmpty(x.InstrumentUid))
                    .ToDictionary(x => x.InstrumentUid, x => Math.Round(
                        decimal.Parse($"{x.Price.Units}.{x.Price.Nano}"), 2, MidpointRounding.AwayFromZero)
                    );
            }
        }
        finally
        {
            Semaphore.Release();
        }

        return _lastPrices;
    }

    private void GetInstrumentsUids(IEnumerable<string> tikers)
    {
        var arr = tikers.Select(x =>
            new
            {
                Tiker = x,
                Uids = apiClient.Instruments.FindInstrument(new FindInstrumentRequest()
                        { Query = x, InstrumentKind = InstrumentType.Share, ApiTradeAvailableFlag = true })
                    .Instruments.Select(i => i.Uid)
            });

        foreach (var a in arr)
        {
            Console.WriteLine(a.Tiker);
            Console.WriteLine(string.Join(", ", a.Uids));
        }
    }
}