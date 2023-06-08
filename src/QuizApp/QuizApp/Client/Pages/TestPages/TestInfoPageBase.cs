using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using QuizApp.Client.Services.Interfaces;
using QuizApp.Shared.Models;

namespace QuizApp.Client.Pages.TestPages;

public class TestInfoPageBase : ComponentBase
{
    [Parameter]
    public Guid Id { get; set; }

    [Inject]
    public ITestService testService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    public Test test = new Test();

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

    protected override async Task OnParametersSetAsync()
    {
        if (Id != Guid.Empty)
        {
            test = await testService.GetTestByIdForUser(Id);
        }
    }

    protected async Task StartTest()
    {
        NavigationManager.NavigateTo("/tests");
    }
}
