using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Api.UserPermissions;

public class AssignPermissionToUserEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/user-permissions/assign", async (
            [FromBody] AssignUserPermissionRequest body,
            UserManager<AppUser> userManager,
            AppDbContext db) =>
        {
            var user = await userManager.FindByIdAsync(body.UserId.ToString());
            if (user == null) return Results.NotFound("User not found");

            var permission = await db.Permissions.FindAsync(body.PermissionId);
            if (permission == null) return Results.NotFound("Permission not found");

            var userGroup = new UserGroup
            {
                UserId = user.Id,
                GroupId = body.PermissionId
            };

            db.UserGroups.Add(userGroup);
            await db.SaveChangesAsync();

            return Results.Ok("Permission assigned to user");
        });
    }
}

public class AssignUserPermissionRequest
{
    public Guid UserId { get; set; }
    public Guid PermissionId { get; set; }
}