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

    /// <summary>Получить количество вложенных денег по брокерам</summary>
    public IEnumerable<Seed> GetSeedMoney()
    {
        int index = 0;
        foreach (var row in MiniExcel.Query(path, sheetName: "общее"))
        {
            if (row.A == null) break;
            index++;
        }
        var num = "A" + index;

        foreach (var item in MiniExcel.QueryRange(path, true, "общее", endCell: num).Cast<IDictionary<string, object>>())
        {
            yield return new Seed
            {
                SeedMoney = (int)(double)item["SeedMoney"],
                Broker = Enum.Parse<Broker>(item["Broker"].ToString()!, true)
            };
        }
    }
}