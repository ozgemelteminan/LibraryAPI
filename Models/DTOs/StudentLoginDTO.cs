using System.ComponentModel.DataAnnotations;

// Data Transfer Object (DTO) used for student login requests
public class StudentLoginDto
{
    // Email address of the student (Required)
    [Required]
    public string Email { get; set; } = null!;

   
    // Password of the student (Required)
    [Required]
    public string Password { get; set; } = null!;
}
