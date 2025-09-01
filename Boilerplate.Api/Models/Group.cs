using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

public class Group : IdentityRole<Guid>
{
    public ICollection<UserGroup> UserGroups { get; set; }
    public ICollection<GroupPermission> GroupPermissions { get; set; }
}
