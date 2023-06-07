using QuizApp.Client.Services.Interfaces;
using QuizApp.Shared.Models;
using System.Net.Http.Json;

namespace QuizApp.Client.Services;

public class TestService : ITestService
{
    private readonly HttpClient _httpClient;

    public TestService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public List<Test> Tests { get; set; } = new List<Test>();

    public async Task<List<Test>> GetAllTests()
    {
        var httpResponse = await _httpClient.GetAsync($"api/test");

        if (httpResponse.IsSuccessStatusCode)
        {
            var result = await httpResponse.Content.ReadFromJsonAsync<List<Test>>();

            if (result != null)
            {
                Tests = result;
            }
        }

        return Tests;
    }

    public async Task<Test> GetTestById(Guid id)
    {
        var result = await _httpClient.GetFromJsonAsync<Test>($"api/test/{id}");

        if (result != null)
        {
            return result;
        }
        else
        {
            throw new Exception("Test not found.");
        }
    }

    public async Task CreateTest(Test test)
    {
        var result = await _httpClient.PostAsJsonAsync("api/test", test);
    }

    public async Task UpdateTest(Test test)
    {
        var result = await _httpClient.PutAsJsonAsync($"api/test", test);
    }

    public async Task DeleteTest(Guid id)
    {
        var result = await _httpClient.DeleteAsync($"api/test/{id}");
    }

    public async Task<List<Test>> GetTestsForUser()
    {
        var httpResponse = await _httpClient.GetAsync($"api/test");

        if (httpResponse.IsSuccessStatusCode)
        {
            var result = await httpResponse.Content.ReadFromJsonAsync<List<Test>>();

            if (result != null)
            {
                Tests = result;
            }
        }

        return Tests;
    }
}
