using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Api.Auth;

public class CreatePermissionEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/permissions", async (
            [FromBody] CreatePermissionRequest body,
            RoleManager<IdentityRole<Guid>> roleManager) =>
        {
            if (string.IsNullOrWhiteSpace(body.Name))
                return Results.BadRequest("Permission name is required.");

            if (await roleManager.RoleExistsAsync(body.Name))
                return Results.BadRequest("Permission already exists.");

            var result = await roleManager.CreateAsync(new IdentityRole<Guid>(body.Name));
            if (!result.Succeeded)
                return Results.BadRequest(result.Errors);

            return Results.Ok($"Permission '{body.Name}' created.");
        });
    }
}

public class CreatePermissionRequest
{
    public string Name { get; set; } = string.Empty;
}