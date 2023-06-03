using QuizApp.Shared.Models;

namespace QuizApp.Shared.DTO;

public class QuestionForTestParticipantDTO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Description { get; set; } = string.Empty;
    public Guid TestId { get; set; }
    public Test? Test { get; set; }
    public List<string> Options { get; set; } = new List<string>();
}
