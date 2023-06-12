using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using QuizApp.Client.Services.Interfaces;
using QuizApp.Shared.Models;

namespace QuizApp.Client.Pages.UserPages;

public class GroupUsersBase : ComponentBase
{
    [Parameter]
    public Guid Id { get; set; }

    [Inject]
    public IUserService userService { get; set; }

    [Inject]
    public IGroupService groupService { get; set; }

    [Inject]
    public ITestParticipantService testParticipantService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [CascadingParameter]
    protected Task<AuthenticationState> AuthenticationState { get; set; }

    public Group? Group { get; set; }

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
                await userService.GetUsersByGroupId(Id);
                Group = await groupService.GetGroupById(Id);
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

    protected async Task DeleteUserFromGroup(Guid userId)
    {
        await testParticipantService.DeleteTestParticipantByGroupIdByUserId(Id, userId);
        await groupService.UnsetUserGroup(userId);
        await userService.GetUsersByGroupId(Id);
        Group = await groupService.GetGroupById(Id);
        NavigationManager.NavigateTo($"/groupusers/{Id}");
    }
}
