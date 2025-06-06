﻿using Microsoft.AspNetCore.Identity;

namespace IdentityApp.Models
{
    public class AppUser:IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => string.Join(" ", FirstName, LastName);
    }
}
