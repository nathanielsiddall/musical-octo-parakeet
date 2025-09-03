using System;

public class Schoolhouse
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public AppUser? Owner { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Subdomain { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Tagline { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public string BannerUrl { get; set; } = string.Empty;
    public string ThemeColor { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
