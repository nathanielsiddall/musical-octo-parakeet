using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Api.Auth;

public class GetGroupsEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/auth/groups", async (AppDbContext db) =>
        {
            var groups = await db.Groups.ToListAsync();
            return Results.Ok(groups);
        });
    }
}