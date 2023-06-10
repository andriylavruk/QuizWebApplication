using QuizApp.Client.Services.Interfaces;
using QuizApp.Shared.Models;
using System.Net.Http.Json;

namespace QuizApp.Client.Services;

public class QuestionService : IQuestionService
{
    private readonly HttpClient _httpClient;

    public QuestionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public List<Question> Questions { get; set; } = new List<Question>();

    public async Task<List<Question>> GetQuestionsByTestId(Guid testId)
    {
        var httpResponse = await _httpClient.GetAsync($"/questionsForTest/{testId}");

        if (httpResponse.IsSuccessStatusCode)
        {
            var result = await httpResponse.Content.ReadFromJsonAsync<List<Question>>();

            if (result != null)
            {
                Questions = result;
            }
        }

        return Questions;
    }

    public async Task<Question> GetQuestionById(Guid id)
    {
        var result = await _httpClient.GetFromJsonAsync<Question>($"api/question/{id}");

        if (result != null)
        {
            return result;
        }
        else
        {
            throw new Exception("Question not found.");
        }
    }

    public async Task<int> GetNumberOfQuestionsByTetsIdForStudent(Guid testId)
    {
        var httpResponse = await _httpClient.GetAsync($"/numberofquestionsintestforuser/{testId}");

        return await httpResponse.Content.ReadFromJsonAsync<int>();
    }

    public async Task CreateQuestion(Question question)
    {
        var result = await _httpClient.PostAsJsonAsync("api/question", question);
    }

    public async Task UpdateQuestion(Question question)
    {
        var result = await _httpClient.PutAsJsonAsync($"api/question", question);
    }

    public async Task DeleteQuestion(Guid id)
    {
        var result = await _httpClient.DeleteAsync($"api/question/{id}");
    }
}
