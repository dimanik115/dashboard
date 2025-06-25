using Dashboard.Extensions;
using Dashboard.Models;
using MiniExcelLibs;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Dashboard.Services;

public class TradeService(IConfiguration configuration)
{
    private readonly string path = configuration.GetValue<string>("ExcelPath")!;

    public async Task<IEnumerable<Trade>> GetAll(QueryParams queryParams)
    {
        var result = MiniExcel.Query<Trade>(path, "сделки")
            .WithParams(queryParams);
        return result.ToList();
    }

    public int GetSeedMoney()
    {
        var firstRow = MiniExcel.Query<Seed>(path, "общее", hasHeader: false).First();

        return firstRow.Money;
    }
}