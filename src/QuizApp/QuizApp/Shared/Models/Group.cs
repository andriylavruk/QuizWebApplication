namespace QuizApp.Shared.Models;

public class Group
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public IEnumerable<User> Users { get; set; } = Enumerable.Empty<User>();
}
