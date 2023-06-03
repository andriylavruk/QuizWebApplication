namespace QuizApp.Shared.Models;

public class Question
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Description { get; set; } = string.Empty;
    public Guid TestId { get; set; }
    public Test? Test { get; set; } 
    public string Option1 { get; set; } = string.Empty;
    public string Option2 { get; set; } = string.Empty;
    public string Option3 { get; set; } = string.Empty;
    public string Option4 { get; set; } = string.Empty;
    public int RightAnswer { get; set; }
}
