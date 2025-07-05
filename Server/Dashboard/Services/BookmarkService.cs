using Dashboard.Models;
using MiniExcelLibs;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Dashboard.Services;

public class BookmarkService(IConfiguration configuration)
{
    private readonly string path = configuration.GetValue<string>("ExcelPath")!;

    /// <summary>Получить все сохраненные закладки</summary>
    public async Task<IEnumerable<Bookmark>> GetAll()
    {
        var result = MiniExcel.Query<Bookmark>(path, "bookmarks").Select((x, i) =>
        {
            x.Id = i + 1;
            return x;
        });
        return result.ToList();
    }
}