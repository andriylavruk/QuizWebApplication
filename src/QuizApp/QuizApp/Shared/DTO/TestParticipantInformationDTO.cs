namespace QuizApp.Shared.DTO;

public class TestParticipantInformationDTO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime? StartedAt { get; set; } = null;
    public DateTime? FinishedAt { get; set; } = null;
    public int? Grade { get; set; } = null;
}
