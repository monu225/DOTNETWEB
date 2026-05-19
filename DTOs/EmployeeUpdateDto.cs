using System.ComponentModel.DataAnnotations;

namespace WEBAPI_CRUD.DTOs;

public sealed class EmployeeUpdateDto
{
    [Required, MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Department { get; set; } = string.Empty;

    [Range(0, 999999999)]
    public decimal Salary { get; set; }

    public DateTime JoiningDate { get; set; }
    public bool IsActive { get; set; } = true;
}
