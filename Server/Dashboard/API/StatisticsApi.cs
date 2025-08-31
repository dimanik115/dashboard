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
        group.MapGet("/byCountry", GetAllStatisticsByCountry);
        group.MapGet("/bySector", GetAllStatisticsBySector);
        group.MapGet("/byType", GetAllStatisticsByType);
        group.MapGet("/totalMoney", GetTotalMoney);

        return group;
    }

    [EndpointSummary("Получить статистику с учетом онлайн")]
    [Produces<IEnumerable<Statistics>>]
    [ProducesResponseType(400)]
    private static async Task<IResult> GetAllStatistics([AsParameters] GetOnlineRequest request)
    {
        var res = await request.Service.GetAll(request.IsOnline);
        var bookmarks = request.BookmarkService.GetAll();
        return Results.Ok(res.Select(s =>
        {
            if (s.Ticker.StartsWith("RU"))
                s.Ticker = $"{s.Ticker} ({bookmarks.First(b => b.Ticker == s.Ticker).Description})";
            return s;
        }));
    }

    [EndpointSummary("Получить статистику по странам с учетом онлайн")]
    [Produces<IEnumerable<CountryStatistics>>]
    [ProducesResponseType(400)]
    private static async Task<IResult> GetAllStatisticsByCountry([AsParameters] GetOnlineRequest request)
    {
        return Results.Ok(await request.Service.GetAllByCountry(request.IsOnline));
    }

    [EndpointSummary("Получить статистику по секторам с учетом онлайн")]
    [Produces<IEnumerable<SectorStatistics>>]
    [ProducesResponseType(400)]
    private static async Task<IResult> GetAllStatisticsBySector([AsParameters] GetOnlineRequest request)
    {
        return Results.Ok(await request.Service.GetAllBySector(request.IsOnline));
    }

    [EndpointSummary("Получить статистику по типу активов с учетом онлайн")]
    [Produces<IEnumerable<TypeStatistics>>]
    [ProducesResponseType(400)]
    private static async Task<IResult> GetAllStatisticsByType([AsParameters] GetOnlineRequest request)
    {
        return Results.Ok(await request.Service.GetAllByType(request.IsOnline));
    }

    [EndpointSummary("Получить сумму денег по всем активам с учетом онлайн")]
    [Produces<decimal>]
    [ProducesResponseType(400)]
    private static async Task<IResult> GetTotalMoney([AsParameters] GetOnlineRequest request)
    {
        return Results.Ok(await request.Service.GetTotalMoney(request.IsOnline));
    }
}