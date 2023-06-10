using QuizApp.Shared.DTO;
using QuizApp.Shared.Models;

namespace QuizApp.Client.Services.Interfaces;

public interface ITestParticipantService
{
    List<TestParticipant>? TestParticipants { get; set; }

    Task<List<TestParticipant>> GetTestParticipantsByTestIdByGroupId(Guid testId, Guid groupId);
    Task<TestParticipantInformationDTO?> GetTestParticipantInfoByTestIdForUser(Guid testId);
    Task AddTestParticipantsByGroupId(Guid testId, Guid groupId);
    Task DeleteTestParticipantsByGroupId(Guid testId, Guid groupId);
}
