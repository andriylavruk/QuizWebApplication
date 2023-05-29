namespace QuizApp.Shared.Models;

public class TestParticipant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid TestId { get; set; }
    public Test? Test { get; set; }
    public int Grade { get; set; } = 0;
    public IEnumerable<TestResult> TestResults { get; set; } = Enumerable.Empty<TestResult>();
}
