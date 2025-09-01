using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Api.Auth;

public class AssignPermissionToGroupEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/groups/{groupId:guid}/permissions", async (
            Guid groupId,
            [FromBody] GroupPermissionRequest body,
            AppDbContext db) =>
        {
            var group = await db.Groups.FindAsync(groupId);
            if (group == null) return Results.NotFound("Group not found.");

            var permission = await db.Permissions.FindAsync(body.PermissionId);
            if (permission == null) return Results.NotFound("Permission not found.");

            var gp = new GroupPermission { GroupId = groupId, PermissionId = body.PermissionId };
            db.GroupPermissions.Add(gp);
            await db.SaveChangesAsync();

            return Results.Ok($"Permission {permission.Name} assigned to group {group.Name}.");
        });
    }
}

public class GroupPermissionRequest
{
    public Guid PermissionId { get; set; }
}