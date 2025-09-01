using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Api.Auth;

public class AssignPermissionEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/permissions/assign", async (
            [FromBody] AssignPermissionRequest body,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager) =>
        {
            var user = await userManager.FindByEmailAsync(body.Email);
            if (user == null) return Results.NotFound("User not found.");

            if (!await roleManager.RoleExistsAsync(body.Permission))
                return Results.NotFound("Permission does not exist.");

            var result = await userManager.AddToRoleAsync(user, body.Permission);
            if (!result.Succeeded)
                return Results.BadRequest(result.Errors);

            return Results.Ok($"Permission '{body.Permission}' assigned to {body.Email}");
        });
    }
}

public class AssignPermissionRequest
{
    public string Email { get; set; } = string.Empty;
    public string Permission { get; set; } = string.Empty;
}