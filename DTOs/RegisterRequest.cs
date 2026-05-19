using System.ComponentModel.DataAnnotations;

namespace WEBAPI_CRUD.DTOs;

public sealed class RegisterRequest
{
    [Required, MaxLength(100)]
    public string UserName { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6), MaxLength(100)]
    public string Password { get; set; } = string.Empty;

    [RegularExpression("Admin|User")]
    public string Role { get; set; } = "User";
}
