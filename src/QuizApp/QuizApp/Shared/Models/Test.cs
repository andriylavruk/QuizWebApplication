namespace QuizApp.Shared.Models;

public class Test
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime AvailableFrom { get; set; }
    public DateTime AvailableUntil { get; set; }
    public DateTime Duration { get; set; }
    public bool IsRandomQuestions { get; set; } = false;
    public bool IsRandomAnswers { get; set; } = false;
    public IEnumerable<Question> Questions { get; set; } = Enumerable.Empty<Question>();
}
