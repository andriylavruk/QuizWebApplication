using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using QuizApp.Client.Pages.QuestionPages;
using QuizApp.Client.Services;
using QuizApp.Client.Services.Interfaces;
using QuizApp.Shared.Models;

namespace QuizApp.Client.Pages.TestPages;

public class TestsBase : ComponentBase
{
    [Inject]
    public IGroupService groupService { get; set; }

    [Inject]
    public ITestService testService { get; set; }

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

                if (user.IsInRole("Administrator"))
                {
                    await testService.GetAllTests();
                }
                else if (user.IsInRole("Student"))
                {
                    await testService.GetTestsForCurrentUser();
                }
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

    protected void ShowTestQuestions(Guid id)
    {
        NavigationManager.NavigateTo($"/testquestions/{id}");
    }

    protected void EditTest(Guid id)
    {
        NavigationManager.NavigateTo($"test/{id}");
    }

    protected void ShowTestGroups(Guid testId)
    {
        NavigationManager.NavigateTo($"/testgroups/{testId}");
    }

    protected void CreateTest()
    {
        NavigationManager.NavigateTo("/test");
    }

    protected void ShowTestInfo(Guid testId)
    {
        NavigationManager.NavigateTo($"/testinfo/{testId}");
    }

    protected async Task DeleteTest(Guid id)
    {
        await testService.DeleteTest(id);
        await testService.GetAllTests();
        NavigationManager.NavigateTo("/tests");
    }
}
