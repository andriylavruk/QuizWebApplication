using QuizApp.Shared.Models;

namespace QuizApp.Shared.DTO;

public class AddTestParticipantDTO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid TestId { get; set; }
    public Test? Test { get; set; }
}
