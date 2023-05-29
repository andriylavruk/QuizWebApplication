namespace QuizApp.Shared.Models;

public class Question
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Description { get; set; } = string.Empty;
    public Guid TestId { get; set; }
    public Test? Test { get; set; }
    public IEnumerable<Answer> Answers { get; set; } = Enumerable.Empty<Answer>();
    public int Number { get; set; }
}
