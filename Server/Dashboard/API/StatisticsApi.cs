using Dashboard.API.Requests;
using Dashboard.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API;

public static class StatisticsApi
{
    public static RouteGroupBuilder MapStatisticsApi(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/statistics");

        group.MapGet("/list", GetAllStatistics);

        return group;
    }

    [EndpointSummary("Получить статистику")]
    [Produces<IEnumerable<Statistics>>]
    [ProducesResponseType(400)]
    private static async Task<IResult> GetAllStatistics([AsParameters] GetAllStatisticsRequests requests)
    {
        return Results.Ok(await requests.Service.GetAll());
    }
}