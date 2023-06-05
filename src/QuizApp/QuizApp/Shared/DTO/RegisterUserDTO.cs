using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace QuizApp.Shared.DTO;

public class RegisterUserDTO
{
    [Required(ErrorMessage = "Обов'язкове поле.")]
    [EmailAddress(ErrorMessage = "Невірний формат.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Обов'язкове поле.")]
    [PasswordPropertyText]
    [StringLength(36, MinimumLength = 8, ErrorMessage = "Довжина пароля повинна бути від 8 до 36 символів.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Обов'язкове поле.")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Довжина імені повинна бути від 3 до 30 символів.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Обов'язкове поле.")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Довжина прізвища повинна бути від 3 до 30 символів.")]
    public string LastName { get; set; } = string.Empty;
}
