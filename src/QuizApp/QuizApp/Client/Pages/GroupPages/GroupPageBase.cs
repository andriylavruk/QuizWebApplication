using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using QuizApp.Client.Services.Interfaces;
using QuizApp.Shared.Models;

namespace QuizApp.Client.Pages.GroupPages;

public class GroupPageBase : ComponentBase
{
    [Parameter]
    public Guid Id { get; set; }

    [Inject]
    public IGroupService groupService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    public string btnText = string.Empty;

    public Group group = new Group();

    [CascadingParameter]
    protected Task<AuthenticationState> AuthenticationState { get; set; }

    protected bool _signInSuccessful = false;

    public string ErrorMessage { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthenticationState).User;

        btnText = Id == Guid.Empty ? "Створити групу" : "Редагувати групу";

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
            group = await groupService.GetGroupById(Id);
        }
    }

    protected async Task HandleSubmit()
    {
        if (Id == Guid.Empty)
        {
            await groupService.CreateGroup(group);
        }
        else
        {
            await groupService.UpdateGroup(group);
        }

        NavigationManager.NavigateTo("/groups");
    }

    protected async Task DeleteGroup()
    {
        await groupService.DeleteGroup(group.Id);
        NavigationManager.NavigateTo("/groups");
    }
}
