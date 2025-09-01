using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Api.Auth;

public class GetPermissionByIdEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/auth/permissions/{id:guid}", async (
            Guid id,
            RoleManager<IdentityRole<Guid>> roleManager) =>
        {
            var permission = await roleManager.FindByIdAsync(id.ToString());
            if (permission == null) return Results.NotFound("Permission not found.");
            return Results.Ok(new { permission.Id, permission.Name });
        });
    }
}