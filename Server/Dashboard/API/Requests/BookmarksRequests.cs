using Dashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Requests;

public record BookmarksRequests
{
    [FromServices]
    public BookmarkService Service { get; set; }
}