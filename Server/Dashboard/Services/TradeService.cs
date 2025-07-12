using Dashboard.Extensions;
using Dashboard.Models;
using MiniExcelLibs;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Dashboard.Services;

public class TradeService(IConfiguration configuration)
{
    private readonly string path = configuration.GetValue<string>("ExcelPath")!;

    /// <summary>Получить все сделки с учетом фильтрации и пагинации</summary> 
    public async Task<List<Trade>> GetAll(QueryParams queryParams)
    {
        var result = MiniExcel.Query<Trade>(path, "сделки").Select((x, i) =>
            {
                x.Id = i + 1;
                return x;
            })
            .WithParams(queryParams);
        return result.ToList();
    }

    /// <summary>Получить количество вложенных денег</summary>
    public int GetSeedMoney()
    {
        var firstRow = MiniExcel.Query<Seed>(path, "общее", hasHeader: false).First();

        return firstRow.Money;
    }
}