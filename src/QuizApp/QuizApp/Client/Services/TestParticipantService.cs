using QuizApp.Client.Services.Interfaces;
using QuizApp.Shared.Models;

namespace QuizApp.Client.Services;

public class TestParticipantService : ITestParticipantService
{
    private readonly HttpClient _httpClient;

    public TestParticipantService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public List<TestParticipant> TestParticipants { get; set; } = new List<TestParticipant>();

    public async Task AddTestParticipantsByGroupId(Guid testId, Guid groupId)
    {
        var result = await _httpClient.PostAsync($"api/testparticipant/addtestparticipants/{testId}/{groupId}", null);
    }

    public async Task DeleteTestParticipantsByGroupId(Guid testId, Guid groupId)
    {
        var result = await _httpClient.PostAsync($"api/testparticipant/deletetestparticipants/{testId}/{groupId}", null);
    }
}
