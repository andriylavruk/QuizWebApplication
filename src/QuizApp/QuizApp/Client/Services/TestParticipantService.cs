﻿using QuizApp.Client.Services.Interfaces;
using QuizApp.Shared.DTO;
using QuizApp.Shared.Models;
using System.Net.Http.Json;

namespace QuizApp.Client.Services;

public class TestParticipantService : ITestParticipantService
{
    private readonly HttpClient _httpClient;

    public TestParticipantService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public List<TestParticipant>? TestParticipants { get; set; }

    public async Task<List<TestParticipant>> GetTestParticipantsByTestIdByGroupId(Guid testId, Guid groupId)
    {
        var httpResponse = await _httpClient.GetAsync($"api/testparticipant/participants/{testId}/{groupId}");

        if (httpResponse.IsSuccessStatusCode)
        {
            var result = await httpResponse.Content.ReadFromJsonAsync<List<TestParticipant>>();

            if (result != null)
            {
                TestParticipants = result;
            }
        }

        return TestParticipants;
    }

    public async Task<TestParticipantInformationDTO?> GetTestParticipantInfoByTestIdForUser(Guid testId)
    {
        var result = await _httpClient.GetFromJsonAsync<TestParticipantInformationDTO>($"/participantforuser/{testId}");

        if (result != null)
        {
            return result;
        }
        else
        {
            throw new Exception("TestParticipantInformationDTO not found.");
        }
    }

    public async Task AddTestParticipantsByTestIdByGroupId(Guid testId, Guid groupId)
    {
        var result = await _httpClient.PostAsync($"api/testparticipant/addtestparticipants/{testId}/{groupId}", null);
    }

    public async Task AddTestParticipantByGroupIdByUserId(Guid groupId, Guid userId)
    {
        var result = await _httpClient.PostAsync($"api/testparticipant/{groupId}/{userId}", null);
    }

    public async Task DeleteTestParticipantsByTestIdByGroupId(Guid testId, Guid groupId)
    {
        var result = await _httpClient.DeleteAsync($"api/testparticipant/deletetestparticipants/{testId}/{groupId}");
    }

    public async Task DeleteTestParticipantByGroupIdByUserId(Guid groupId, Guid userId)
    {
        var result = await _httpClient.DeleteAsync($"api/testparticipant/{groupId}/{userId}");
    }
}
