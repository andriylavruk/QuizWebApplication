using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using QuizApp.Client.Providers;
using QuizApp.Shared.DTO;
using System.Net.Http.Json;

namespace QuizApp.Client.Pages.AuthPages;

public class SignInBase : ComponentBase
{
    [CascadingParameter]
    protected Task<AuthenticationState> AuthenticationState { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;

    [Inject]
    public HttpClient HttpClient { get; set; }

    [Inject]
    public ILocalStorageService LocalStorageService { get; set; }

    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    protected LoginUserDTO _userToSignIn = new();
    protected bool _signInSuccessful = false;
    protected bool _attemptToSignInFailed = false;

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    protected async Task SignInUser()
    {
        _attemptToSignInFailed = false;

        HttpResponseMessage httpResponseMessage = await HttpClient.PostAsJsonAsync("api/auth/login", _userToSignIn);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            string jsonWebToken = await httpResponseMessage.Content.ReadAsStringAsync();

            await LocalStorageService.SetItemAsync("bearerToken", jsonWebToken);

            await ((AppAuthenticationStateProvider)AuthenticationStateProvider).SignIn();

            HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearerToken", jsonWebToken);

            _signInSuccessful = true;
        }
        else
        {
            _attemptToSignInFailed = true;
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
