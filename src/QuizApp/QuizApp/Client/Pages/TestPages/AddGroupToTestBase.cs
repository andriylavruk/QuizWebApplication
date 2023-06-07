using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using QuizApp.Client.Services.Interfaces;
using QuizApp.Shared.DTO;
using QuizApp.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace QuizApp.Client.Pages.TestPages;

public class AddGroupToTestBase : ComponentBase
{
    [Parameter]
    public Guid Id { get; set; }

    [Inject]
    public IGroupService groupService { get; set; }

    [Inject]
    public ITestParticipantService testParticipantService { get; set; }

    [Inject]
    public ITestService testService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    public Test test = new Test();

    public List<Group> groups = new List<Group>();

    public AddGroupToTestDTO group = new AddGroupToTestDTO();

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
                groups = await groupService.GetGroupsToAddByTestId(Id);
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
            test = await testService.GetTestById(Id);
        }
    }

    protected async Task AddGroupToTest()
    {
        await testParticipantService.AddTestParticipantsByGroupId(Id, new Guid(group.Id.ToString()!));
        NavigationManager.NavigateTo($"/testgroups/{Id}");
    }
}
