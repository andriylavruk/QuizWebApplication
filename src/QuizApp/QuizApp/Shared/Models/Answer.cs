namespace QuizApp.Shared.Models;

public class Answer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Description { get; set; } = string.Empty;
    public Guid QuestionId { get; set; }
    public Question? Question { get; set; }
    public bool IsRightAnswer { get; set; }
}
