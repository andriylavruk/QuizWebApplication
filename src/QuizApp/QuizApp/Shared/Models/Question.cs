using System.ComponentModel.DataAnnotations;

namespace QuizApp.Shared.Models;

public class Question
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "Обов'язкове поле.")]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Довжина запитання повинна бути від {2} до {1} символів.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Обов'язкове поле.")]
    public Guid TestId { get; set; }
    public Test? Test { get; set; }

    [Required(ErrorMessage = "Обов'язкове поле.")]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Довжина відповіді 1 повинна бути від {2} до {1} символів.")]
    public string Option1 { get; set; } = string.Empty;

    [Required(ErrorMessage = "Обов'язкове поле.")]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Довжина відповіді 2 повинна бути від {2} до {1} символів.")]
    public string Option2 { get; set; } = string.Empty;

    [Required(ErrorMessage = "Обов'язкове поле.")]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Довжина відповіді 3 повинна бути від {2} до {1} символів.")]
    public string Option3 { get; set; } = string.Empty;

    [Required(ErrorMessage = "Обов'язкове поле.")]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Довжина відповіді 4 повинна бути від {2} до {1} символів.")]
    public string Option4 { get; set; } = string.Empty;

    [Required(ErrorMessage = "Обов'язкове поле.")]
    [Range(1, 4, ErrorMessage = "Число повинно бути у межах від 1 до 4.")]
    public int RightAnswer { get; set; } = 1;
}
