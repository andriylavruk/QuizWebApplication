using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using QuizApp.Client.Services.Interfaces;

namespace QuizApp.Client.Pages.GroupPages;

public class GroupsBase : ComponentBase
{
    [Inject]
    public IGroupService groupService { get; set; }

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
                await groupService.GetAllGroups();
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

    protected void ShowGroup(Guid id)
    {
        NavigationManager.NavigateTo($"group/{id}");
    }

    protected void CreateEditGroup()
    {
        NavigationManager.NavigateTo("/group");
    }

    protected async Task DeleteGroup(Guid id)
    {
        await groupService.DeleteGroup(id);
        await groupService.GetAllGroups();
        NavigationManager.NavigateTo("/groups");
    }
}
