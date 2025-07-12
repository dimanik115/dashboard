using Dashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Requests;

public record CurrencyRequest
{
    [FromServices]
    public CurrencyService Service { get; set; }
    
    [FromQuery]
    public bool IsOnline { get; set; } 
}