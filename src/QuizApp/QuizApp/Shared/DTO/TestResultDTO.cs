using QuizApp.Shared.Models;

namespace QuizApp.Shared.DTO;

public class TestResultDTO
{
    public required UserDTO User { get; set; }
    public required Test Test { get; set; }
    public int Grade { get; set; }
    public int NumberOfQuestions { get; set; }
}
