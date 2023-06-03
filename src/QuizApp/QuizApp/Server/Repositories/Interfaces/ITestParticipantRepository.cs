using QuizApp.Shared.Models;

namespace QuizApp.Server.Repositories.Interfaces;

public interface ITestParticipantRepository
{
    Task<List<TestParticipant>> GetTestParticipantsByGroupIdAsunc(Guid groupId);
    Task<TestParticipant?> GetTestParticipantByIdAsync(Guid id);
    Task<TestParticipant?> GetTestParticipantByTestIdAsync(Guid testId);
    Task<bool> IsTestParticipantExistAsync(Guid id);
    Task<bool> AddTestParticipantsByGroupIdAsync(Guid testId, Guid groupId);
    Task<bool> UpdateTestParticipantAsync(TestParticipant testParticipant, Guid currentUserId);
    Task<bool> DeleteTestParticipantsByGroupIdAsync(Guid testId, Guid groupId);
}
