namespace QuizApp.Shared.DTO;

public class AnswersToQuestionsDTO
{
    public Guid UserId { get; set; }
    public Guid TestId { get; set; }
    public List<int> Answers { get; set; } = new List<int>();
}
