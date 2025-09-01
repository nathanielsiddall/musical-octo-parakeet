using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Api.Auth;

public class RemoveGroupPermissionEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/auth/groups/{groupId:guid}/permissions/{permissionId:guid}", async (
            Guid groupId,
            Guid permissionId,
            AppDbContext db) =>
        {
            var gp = await db.GroupPermissions.FindAsync(groupId, permissionId);
            if (gp == null) return Results.NotFound();

            db.GroupPermissions.Remove(gp);
            await db.SaveChangesAsync();

            return Results.Ok("Permission removed from group.");
        });
    }
}