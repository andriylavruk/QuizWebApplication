using QuizApp.Shared.DTO;
using QuizApp.Shared.Models;

namespace QuizApp.Server.Repositories.Interfaces;

public interface IQuestionRepository
{
    Task<List<Question>> GetAllQuestionsAsync();
    Task<List<QuestionForTestParticipantDTO>> GetQuestionsForTestParticipantAsync(Guid id);
    Task<List<Question>> GetQuestionsByTestIdAsync(Guid testId);
    Task<Question?> GetQuestionByIdAsync(Guid questionId);
    Task<bool> IsQuestionExistAsync(Guid id);
    Task<bool> CreateQuestionAsync(Question question);
    Task<bool> UpdateQuestionAsync(Question question);
    Task<bool> DeleteQuestionAsync(Question question);
}
