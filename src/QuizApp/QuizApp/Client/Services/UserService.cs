using QuizApp.Client.Services.Interfaces;
using QuizApp.Shared.DTO;
using System.Net.Http.Json;

namespace QuizApp.Client.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public List<UserDTO> Users { get; set; } = new List<UserDTO>();

    public async Task<List<UserDTO>> GetUsersByGroupId(Guid groupId)
    {
        var httpResponse = await _httpClient.GetAsync($"api/user/groupusers/{groupId}");

        if (httpResponse.IsSuccessStatusCode)
        {
            var result = await httpResponse.Content.ReadFromJsonAsync<List<UserDTO>>();

            if (result != null)
            {
                Users = result;
            }
        }

        return Users;
    }

    public async Task<UserDTO> GetUserById(Guid id)
    {
        var result = await _httpClient.GetFromJsonAsync<UserDTO>($"api/user/{id}");

        if (result != null)
        {
            return result;
        }
        else
        {
            throw new Exception("User not found.");
        }
    }

    public async Task<List<UserDTO>> GetUsersWithoutGroup()
    {
        var httpResponse = await _httpClient.GetAsync($"/userswithoutgroup");

        if (httpResponse.IsSuccessStatusCode)
        {
            var result = await httpResponse.Content.ReadFromJsonAsync<List<UserDTO>>();

            if (result != null)
            {
                Users = result;
            }
        }

        return Users;
    }
}
