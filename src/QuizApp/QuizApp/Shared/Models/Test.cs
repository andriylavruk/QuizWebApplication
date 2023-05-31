namespace QuizApp.Shared.Models;

public class Test
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; }
    public DateTime FinishedAt { get; set; }
    public IEnumerable<Question> Questions { get; set; } = Enumerable.Empty<Question>();
}
