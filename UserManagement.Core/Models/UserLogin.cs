﻿using System.ComponentModel.DataAnnotations;

namespace UserManagement.Core.Models
{
    public class UserLogin
    {
        [Required(ErrorMessage = "UserName is required")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

    }
}
