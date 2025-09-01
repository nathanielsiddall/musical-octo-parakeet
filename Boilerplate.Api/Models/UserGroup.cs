using System;

public class UserGroup
{
    public Guid UserId { get; set; }
    public AppUser User { get; set; }
    public Guid GroupId { get; set; }
    public Group Group { get; set; }
}
