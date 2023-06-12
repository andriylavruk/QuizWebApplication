using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using QuizApp.Client.Services.Interfaces;

namespace QuizApp.Client.Pages.UserPages;

public class UsersWithoutGroupBase : ComponentBase
{
    [Inject]
    public IUserService userService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [CascadingParameter]
    protected Task<AuthenticationState> AuthenticationState { get; set; }


    protected bool _signInSuccessful = false;

    public string ErrorMessage { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthenticationState).User;

        try
        {
            if (user.Identity.IsAuthenticated == true)
            {
                _signInSuccessful = true;
                await userService.GetUsersWithoutGroup();
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

    protected async Task ShowUser(Guid userId)
    {
        await userService.GetUserById(userId);
        NavigationManager.NavigateTo($"/user/{userId}");
    }
}
