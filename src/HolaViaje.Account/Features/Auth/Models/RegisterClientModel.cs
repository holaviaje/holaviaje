using System.ComponentModel.DataAnnotations;

namespace HolaViaje.Account.Features.Auth.Models;

public class RegisterClientModel
{
    [Required]
    [Display(Name = "Client Id")]
    public required string ClientId { get; set; }
    [Required]
    [Display(Name = "Client Secret")]
    public required string ClientSecret { get; set; }
    [Required]
    [Display(Name = "Client Name")]
    public required string ClientName { get; set; }
}
