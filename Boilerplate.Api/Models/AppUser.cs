using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

public class AppUser : IdentityUser<Guid>
{
    public ICollection<UserGroup> UserGroups { get; set; }
}
