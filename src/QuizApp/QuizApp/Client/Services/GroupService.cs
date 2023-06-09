﻿using QuizApp.Client.Services.Interfaces;
using QuizApp.Shared.Models;
using System.Net.Http.Json;

namespace QuizApp.Client.Services;

public class GroupService : IGroupService
{
    private readonly HttpClient _httpClient;

    public GroupService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public List<Group>? Groups { get; set; }

    public async Task<List<Group>> GetAllGroups()
    {
        var httpResponse = await _httpClient.GetAsync($"api/group");

        if (httpResponse.IsSuccessStatusCode)
        {
            var result = await httpResponse.Content.ReadFromJsonAsync<List<Group>>();

            if (result != null)
            {
                Groups = result;
            }
        }

        return Groups;
    }

    public async Task<Group> GetGroupById(Guid id)
    {
        var result = await _httpClient.GetFromJsonAsync<Group>($"api/group/{id}");

        if (result != null)
        {
            return result;
        }
        else
        {
            throw new Exception("Group not found.");
        }
    }

    public async Task<List<Group>> GetGroupsByTestId(Guid testId)
    {
        var httpResponse = await _httpClient.GetAsync($"/testgroups/{testId}");

        if (httpResponse.IsSuccessStatusCode)
        {
            var result = await httpResponse.Content.ReadFromJsonAsync<List<Group>>();

            if (result != null)
            {
                Groups = result;
            }
        }

        return Groups;
    }

    public async Task<List<Group>> GetGroupsToAddByTestId(Guid testId)
    {
        var httpResponse = await _httpClient.GetAsync($"/groupstoaddtotest/{testId}");

        if (httpResponse.IsSuccessStatusCode)
        {
            var result = await httpResponse.Content.ReadFromJsonAsync<List<Group>>();

            if (result != null)
            {
                Groups = result;
            }
        }

        return Groups;
    }

    public async Task CreateGroup(Group group)
    {
        var result = await _httpClient.PostAsJsonAsync("api/group", group);
    }

    public async Task UpdateGroup(Group group)
    {
        var result = await _httpClient.PutAsJsonAsync($"api/group", group);
    }

    public async Task DeleteGroup(Guid id)
    {
        var result = await _httpClient.DeleteAsync($"api/group/{id}");
    }

    public async Task SetUserGroup(Guid userId, Guid groupId)
    {
        var result = await _httpClient.PostAsync($"api/group/setusergroup/{userId}/{groupId}", null);
    }

    public async Task UnsetUserGroup(Guid userId)
    {
        var result = await _httpClient.PostAsync($"api/group/unsetusergroup/{userId}", null);
    }
}
