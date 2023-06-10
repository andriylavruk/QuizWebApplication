using QuizApp.Shared.Models;

namespace QuizApp.Shared.DTO;

public class TestResultDTO
{
    public UserDTO User { get; set; }
    public Test Test { get; set; }
    public int Grade { get; set; }
    public int NumberOfQuestions { get; set; }
}
