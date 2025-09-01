using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Api.Auth;

public class RemoveUserPermissionEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/auth/users/{userId:guid}/permissions/{permissionId:guid}", async (
            Guid userId,
            Guid permissionId,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager) =>
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null) return Results.NotFound("User not found.");

            var permission = await roleManager.FindByIdAsync(permissionId.ToString());
            if (permission == null) return Results.NotFound("Permission not found.");

            var result = await userManager.RemoveFromRoleAsync(user, permission.Name!);
            if (!result.Succeeded) return Results.BadRequest(result.Errors);

            return Results.Ok($"Permission '{permission.Name}' removed from {user.Email}.");
        });
    }
}