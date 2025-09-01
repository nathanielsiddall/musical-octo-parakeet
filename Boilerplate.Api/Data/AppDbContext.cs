using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
    public DbSet<Group> Groups { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<GroupPermission> GroupPermissions { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }
    public DbSet<ForensicLog> ForensicLogs { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserGroup>().HasKey(ug => new { ug.UserId, ug.GroupId });
        builder.Entity<GroupPermission>().HasKey(gp => new { gp.GroupId, gp.PermissionId });
    }
}