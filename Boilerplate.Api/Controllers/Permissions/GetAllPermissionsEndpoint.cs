using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Api.Auth;

public class GetAllPermissionsEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/auth/permissions", async (RoleManager<IdentityRole<Guid>> roleManager) =>
        {
            var permissions = roleManager.Roles.Select(r => new { r.Id, r.Name }).ToList();
            return Results.Ok(permissions);
        });
    }
}