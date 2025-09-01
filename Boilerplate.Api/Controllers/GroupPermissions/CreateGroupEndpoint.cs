using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Api.Auth;

public class CreateGroupEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/groups", async (
            [FromBody] GroupRequest body,
            AppDbContext db) =>
        {
            if (string.IsNullOrWhiteSpace(body.Name))
                return Results.BadRequest("Group name is required.");

            var group = new Group { Id = Guid.NewGuid(), Name = body.Name };
            db.Groups.Add(group);
            await db.SaveChangesAsync();

            return Results.Ok(group);
        });
    }
}

public class GroupRequest
{
    public string Name { get; set; } = string.Empty;
}