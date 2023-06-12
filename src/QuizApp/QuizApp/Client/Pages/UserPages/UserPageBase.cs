using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using QuizApp.Client.Services.Interfaces;
using QuizApp.Shared.DTO;

namespace QuizApp.Client.Pages.UserPages;

public class UserPageBase : ComponentBase
{
    [Parameter]
    public Guid Id { get; set; }

    [Inject]
    public IGroupService groupService { get; set; }

    [Inject]
    public IUserService userService { get; set; }

    [Inject]
    public ITestParticipantService testParticipantService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    public UserDTO user = new UserDTO();

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

    protected override async Task OnParametersSetAsync()
    {
        if (Id != Guid.Empty)
        {
            user = await userService.GetUserById(Id);
        }
    }

    protected async Task UpdateUser()
    {
        await groupService.SetUserGroup(user.Id, new Guid(user.GroupId.ToString()!));
        await testParticipantService.AddTestParticipantByGroupIdByUserId(new Guid(user.GroupId.ToString()!), user.Id);
        NavigationManager.NavigateTo("/studentswithoutgroup");
    }
}
