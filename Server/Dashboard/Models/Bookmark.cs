using MiniExcelLibs.Attributes;

namespace Dashboard.Models;

public class Bookmark
{
    [ExcelIgnore]
    public int Id { get; set; }

    public string Source { get; set; } = null!;

    public string Ticker { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Description { get; set; }
}