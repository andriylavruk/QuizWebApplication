using QuizApp.Shared.DTO;
using QuizApp.Shared.Models;

namespace QuizApp.Client.Services.Interfaces;

public interface ITestParticipantService
{
    List<TestParticipant>? TestParticipants { get; set; }

    Task<List<TestParticipant>> GetTestParticipantsByTestIdByGroupId(Guid testId, Guid groupId);
    Task<TestParticipantInformationDTO?> GetTestParticipantInfoByTestIdForUser(Guid testId);
    Task AddTestParticipantsByTestIdByGroupId(Guid testId, Guid groupId);
    Task AddTestParticipantByGroupIdByUserId(Guid groupId, Guid userId);
    Task DeleteTestParticipantsByTestIdByGroupId(Guid testId, Guid groupId);
    Task DeleteTestParticipantByGroupIdByUserId(Guid groupId, Guid userId);
}
