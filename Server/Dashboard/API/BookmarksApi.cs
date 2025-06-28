using Dashboard.API.Requests;
using Dashboard.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API;

public static class BookmarksApi
{
    public static RouteGroupBuilder MapBookmarksApi(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/bookmarks");

        group.MapGet("/list", GetAllBookmarks);

        return group;
    }

    [EndpointSummary("Получить все сохраненные закладки")]
    [Produces<IEnumerable<Bookmark>>]
    [ProducesResponseType(400)]
    private static async Task<IResult> GetAllBookmarks([AsParameters] BookmarksRequests requests)
    {
        return Results.Ok(await requests.Service.GetAll());
    }
}