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
    public DbSet<Schoolhouse> Schoolhouses { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserGroup>().HasKey(ug => new { ug.UserId, ug.GroupId });
        builder.Entity<GroupPermission>().HasKey(gp => new { gp.GroupId, gp.PermissionId });

        builder.Entity<Schoolhouse>()
            .HasIndex(s => s.Name)
            .IsUnique();

        builder.Entity<Schoolhouse>()
            .HasIndex(s => s.Subdomain)
            .IsUnique();

        builder.Entity<Schoolhouse>()
            .HasOne(s => s.Owner)
            .WithMany()
            .HasForeignKey(s => s.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}