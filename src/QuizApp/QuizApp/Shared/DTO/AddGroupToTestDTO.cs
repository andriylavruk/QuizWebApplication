using System.ComponentModel.DataAnnotations;

namespace QuizApp.Shared.DTO;

public class AddGroupToTestDTO
{
    [Required(ErrorMessage = "Обов'язкове поле.")]
    public Guid? Id { get; set; }
}
