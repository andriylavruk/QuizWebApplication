using QuizApp.Shared.Models;

namespace QuizApp.Server.Repositories.Interfaces;

public interface ITestParticipantRepository
{
    Task<List<TestParticipant>> GetTestParticipantsByGroupIdAsunc(Guid groupId);
    Task<TestParticipant?> GetTestParticipantByIdAsync(Guid id);
    Task<List<TestParticipant>> GetTestParticipantsByTestIdByGroupIdAsync(Guid testId, Guid groupId);
    Task<TestParticipant?> GetTestParticipantByTestIdByUserIdAsync(Guid testId, Guid userId);
    Task<IEnumerable<TestParticipant>> GetTestParticipantsByGroupIdByUserIdAsync(Guid groupId, Guid userId);
    Task<bool> IsTestParticipantExistAsync(Guid id);
    Task<bool> AddTestParticipantByGroupIdByUserIdAsync(Guid groupId, Guid userId);
    Task<bool> AddTestParticipantsByTestIdByGroupIdAsync(Guid testId, Guid groupId);
    Task<bool> UpdateTestParticipantAsync(TestParticipant testParticipant, Guid currentUserId);
    Task<bool> DeleteTestParticipantByGroupIdByUserIdAsync(Guid groupId, Guid userId);
    Task<bool> DeleteTestParticipantsByTestIdByGroupIdAsync(Guid testId, Guid groupId);
}
