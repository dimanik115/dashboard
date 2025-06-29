using Dashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Requests;

public record GetAllStatisticsRequests
{
    [FromServices]
    public StatisticsService Service { get; set; } = null!;
}