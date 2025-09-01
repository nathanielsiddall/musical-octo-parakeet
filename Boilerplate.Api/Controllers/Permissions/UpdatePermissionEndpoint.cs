using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Api.Auth;

public class UpdatePermissionEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/auth/permissions/{id:guid}", async (
            Guid id,
            [FromBody] UpdatePermissionRequest body,
            RoleManager<IdentityRole<Guid>> roleManager) =>
        {
            var permission = await roleManager.FindByIdAsync(id.ToString());
            if (permission == null) return Results.NotFound("Permission not found.");

            permission.Name = body.NewName;
            var result = await roleManager.UpdateAsync(permission);

            if (!result.Succeeded)
                return Results.BadRequest(result.Errors);

            return Results.Ok($"Permission renamed to '{body.NewName}'.");
        });
    }
}

public class UpdatePermissionRequest
{
    public string NewName { get; set; } = string.Empty;
}