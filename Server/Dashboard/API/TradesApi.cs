using Dashboard.API.Requests;
using Dashboard.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API;

public static class TradesApi
{
    public static RouteGroupBuilder MapTradesApi(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/trades");

        group.MapPost("/list", GetAllTrades);
        group.MapGet("/getseedmoney", GetSeedMoney);

        return group;
    }

    [EndpointSummary("Получить количество вложенных денег")]
    [Produces<int>]
    [ProducesResponseType(400)]
    private static Results<Ok<int>, BadRequest<string>> GetSeedMoney([AsParameters] GetSeedMoneyRequest requests)
    {
        return TypedResults.Ok(requests.Service.GetSeedMoney());
    }

    [EndpointSummary("Получить все сделки с учетом фильтрации и пагинации")]
    [Produces<IEnumerable<Trade>>]
    [ProducesResponseType(400)]
    private static async Task<IResult> GetAllTrades([AsParameters] GetAllTradesRequests requests)
    {
        return Results.Ok(await requests.Service.GetAll(requests.Query));
    }
}