using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using QuizApp.Client.Services.Interfaces;

namespace QuizApp.Client.Pages.GroupPages;

public class TestGroupsBase : ComponentBase
{
    [Parameter]
    public Guid Id { get; set; }

    [Inject]
    public IGroupService groupService { get; set; }

    [Inject]
    public ITestParticipantService testParticipantService { get; set; }

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
                await groupService.GetGroupsByTestId(Id);
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

    protected void AddGroupToTest()
    {
        NavigationManager.NavigateTo($"/testgroup/{Id}");
    }

    protected void ShowGroupUsers(Guid groupId)
    {
        NavigationManager.NavigateTo($"/groupusers/{groupId}");
    }

    protected async Task DeleteGroupFromTest(Guid groupId)
    {
        await testParticipantService.DeleteTestParticipantsByGroupId(Id, groupId);
        await groupService.GetGroupsByTestId(Id);
        NavigationManager.NavigateTo($"/testgroups/{Id}");
    }
}
