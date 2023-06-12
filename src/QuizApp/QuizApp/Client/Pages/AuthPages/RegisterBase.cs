using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using QuizApp.Shared.DTO;
using System.Net.Http.Json;

namespace QuizApp.Client.Pages.AuthPages;

public class RegisterBase : ComponentBase
{
    [CascadingParameter]
    protected Task<AuthenticationState> AuthenticationState { get; set; }

    protected bool _signInSuccessful = false;

    public string ErrorMessage { get; set; } = string.Empty;

    [Inject]
    public HttpClient HttpClient { get; set; }

    protected RegisterUserDTO _userToRegister = new();
    protected bool _registerSuccessful = false;
    protected bool _attemptToRegisterFailed = false;
    protected string? _attemptToRegisterFailedErrorMessage = null;

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    protected async Task RegisterUser()
    {
        _attemptToRegisterFailed = false;

        HttpResponseMessage httpResponseMessage = await HttpClient.PostAsJsonAsync("api/auth/register", _userToRegister);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            _registerSuccessful = true;
        }
        else
        {
            string serverErrorMessages = await httpResponseMessage.Content.ReadAsStringAsync();

            _attemptToRegisterFailedErrorMessage = $"{serverErrorMessages} Спробуйте ще раз.";

            _attemptToRegisterFailed = true;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthenticationState).User;

        try
        {
            if (user.Identity.IsAuthenticated == true)
            {
                _signInSuccessful = true;
            }
            else
            {
                _signInSuccessful = false;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}
