using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Api.Auth;

public class UpdateUserPermissionEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/auth/users/{userId:guid}/permissions", async (
            Guid userId,
            [FromBody] UpdateUserPermissionRequest body,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager) =>
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null) return Results.NotFound("User not found.");

            var oldPermission = await roleManager.FindByIdAsync(body.OldPermissionId.ToString());
            if (oldPermission == null) return Results.NotFound("Old permission not found.");

            var newPermission = await roleManager.FindByIdAsync(body.NewPermissionId.ToString());
            if (newPermission == null) return Results.NotFound("New permission not found.");

            var removeResult = await userManager.RemoveFromRoleAsync(user, oldPermission.Name!);
            if (!removeResult.Succeeded) return Results.BadRequest(removeResult.Errors);

            var addResult = await userManager.AddToRoleAsync(user, newPermission.Name!);
            if (!addResult.Succeeded) return Results.BadRequest(addResult.Errors);

            return Results.Ok($"Permission updated from '{oldPermission.Name}' to '{newPermission.Name}' for {user.Email}.");
        });
    }
}

public class UpdateUserPermissionRequest
{
    public Guid OldPermissionId { get; set; }
    public Guid NewPermissionId { get; set; }
}