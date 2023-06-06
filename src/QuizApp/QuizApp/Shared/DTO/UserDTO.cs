using QuizApp.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace QuizApp.Shared.DTO;

public class UserDTO
{
    public Guid Id { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    public Role? Role { get; set; }
    [Required]
    [StringLength(30, MinimumLength = 3)]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [StringLength(30, MinimumLength = 3)]
    public string LastName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Обов'язкове поле.")]
    public Guid? GroupId { get; set; }
    public Group? Group { get; set; }
}
