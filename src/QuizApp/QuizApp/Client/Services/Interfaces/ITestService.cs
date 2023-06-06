using QuizApp.Shared.DTO;
using QuizApp.Shared.Models;

namespace QuizApp.Client.Services.Interfaces;

public interface ITestService
{
    List<Test> Tests { get; set; }

    Task<List<Test>> GetAllTests();
    Task<Test> GetTestById(Guid id);
    Task CreateTest(Test test);
    Task UpdateTest(Test test);
    Task DeleteTest(Guid id);

    /*Task<List<Test>> GetTestsForUser();
    Task<Test> GetTestById();
    Task<Test> GetTestByIdForUser(Guid id);

    Task<List<QuestionForTestParticipantDTO>> StartTest(Guid testId);
    Task<List<TestResultDTO>> FinishTest(Guid testId, AnswersToQuestionsDTO answersToQuestionsDTO);*/
}
