namespace QuizApp.Shared.Models;

public class TestParticipant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid TestId { get; set; }
    public Test? Test { get; set; }
    public DateTime? StartedAt { get; set; } = null;
    public DateTime? FinishedAt { get; set; } = null;
    public int? Grade { get; set; } = null;
}
