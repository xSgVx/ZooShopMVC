﻿using Microsoft.AspNetCore.Identity;

namespace ZooShopMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = null!;

    }
}
