using Dashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Requests;

public record GetRequest
{
    [FromServices]
    public StatisticsService Service { get; set; } = null!;
}

public record GetOnlineRequest
{
    [FromServices]
    public StatisticsService Service { get; set; } = null!;

    [FromQuery]
    public bool IsOnline { get; set; }
}