using Dashboard.Models;
using Dashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Requests;

public record GetAllTradesRequests
{
    public TradeService Service { get; set; }

    [FromBody]
    public QueryParams Query { get; set; }
}

public record GetSeedMoneyRequest
{
    public TradeService Service { get; set; }
}