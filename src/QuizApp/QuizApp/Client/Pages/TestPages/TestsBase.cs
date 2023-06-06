using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using QuizApp.Client.Services.Interfaces;

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
                await testService.GetAllTests();
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

    protected void ShowTest(Guid id)
    {
        NavigationManager.NavigateTo($"test/{id}");
    }

    /*protected void ShowGroupUsers(Guid groupId)
    {
        NavigationManager.NavigateTo($"/groupusers/{groupId}");
    }*/

    protected void CreateEditTest()
    {
        NavigationManager.NavigateTo("/test");
    }

    protected async Task DeleteTest(Guid id)
    {
        await testService.DeleteTest(id);
        await testService.GetAllTests();
        NavigationManager.NavigateTo("/tests");
    }
}
