using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace QuizApp.Shared.DTO;

public class LoginUserDTO
{
    [Required(ErrorMessage = "Обов'язкове поле.")]
    [EmailAddress(ErrorMessage = "Невірний формат.")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Обов'язкове поле.")]
    [PasswordPropertyText]
    public string Password { get; set; } = string.Empty;
}
