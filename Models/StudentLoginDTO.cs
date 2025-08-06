using System.ComponentModel.DataAnnotations;

public class StudentLoginDto
{
    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
