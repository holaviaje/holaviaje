﻿using System.ComponentModel.DataAnnotations;

namespace HolaViaje.Account.Features.Identity.Models;

public class RegisterModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public required string Email { get; set; }

    [Required]
    [Display(Name = "UserName")]
    public required string UserName { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public required string Password { get; set; }
}
