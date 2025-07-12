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
        return Results.Ok(await request.Service.GetAvgPrices(request.IsOnline));
    }
}