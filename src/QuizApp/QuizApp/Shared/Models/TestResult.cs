namespace QuizApp.Shared.Models;

public class TestResult
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TestParticipantId { get; set; }
    public TestParticipant? TestParticipant { get; set; }
    public Guid TestQuestionId { get; set; }
    public Question? Question { get; set; }
    public Guid SelectedAnswerId { get; set; }
    public Answer? SelectedAnswer { get; set; }
}
