using QuizApp.Shared.Models;

namespace QuizApp.Client.Services.Interfaces;

public interface IQuestionService
{
    List<Question>? Questions { get; set; }

    Task<List<Question>> GetQuestionsByTestId(Guid testId);
    Task<Question> GetQuestionById(Guid id);
    Task<int> GetNumberOfQuestionsByTetsIdForStudent(Guid testId);
    Task CreateQuestion(Question question);
    Task UpdateQuestion(Question question);
    Task DeleteQuestion(Guid id);
}
