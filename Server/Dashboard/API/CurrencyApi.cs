using Dashboard.API.Requests;
using Dashboard.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API;

public static class CurrencyApi
{
    public static RouteGroupBuilder MapCurrencyApi(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/currency");

        group.MapGet("/avgPrices", GetAvgPrices);

        return group;
    }

    [EndpointSummary("Получить все средние курсы валют")]
    [Produces<IEnumerable<CurrencyAvgPrice>>]
    [ProducesResponseType(400)]
    private static async Task<IResult> GetAvgPrices([AsParameters] CurrencyRequest request)
    {
        var res = await request.Service.GetAvgPrices(request.IsOnline);
        return Results.Ok(res.Where(x => x.Key != Currency.RUB).Select(x => x.Value));
    }
}