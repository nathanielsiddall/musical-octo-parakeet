using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Api.Auth;

public class DeletePermissionEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/auth/permissions/{id:guid}", async (
            Guid id,
            RoleManager<IdentityRole<Guid>> roleManager) =>
        {
            var permission = await roleManager.FindByIdAsync(id.ToString());
            if (permission == null) return Results.NotFound("Permission not found.");

            var result = await roleManager.DeleteAsync(permission);
            if (!result.Succeeded)
                return Results.BadRequest(result.Errors);

            return Results.Ok($"Permission '{permission.Name}' deleted.");
        });
    }
}