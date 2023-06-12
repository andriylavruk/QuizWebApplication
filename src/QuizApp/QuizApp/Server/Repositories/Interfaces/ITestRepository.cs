using QuizApp.Shared.Models;

namespace QuizApp.Server.Repositories.Interfaces;

public interface ITestRepository
{
    Task<List<Test>> GetAllTestsAsync();
    Task<List<Test>> GetTestsForUserAsync(Guid userId);
    Task<Test?> GetTestByIdAsync(Guid testId);
    Task<bool> IsTestExistAsync(Guid testid);
    Task<bool> CreateTestAsync(Test test);
    Task<bool> UpdateTestAsync(Test test);
    Task<bool> DeleteTestAsync(Test test);
}
