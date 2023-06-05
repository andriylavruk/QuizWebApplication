using System.ComponentModel.DataAnnotations;

namespace QuizApp.Shared.Models;

public class Group
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "Обов'язкове поле.")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Довжина імені повинна бути від 3 до 30 символів.")]
    public string Name { get; set; } = string.Empty;
}
