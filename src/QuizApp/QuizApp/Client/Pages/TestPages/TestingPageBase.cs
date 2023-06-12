using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using QuizApp.Client.Services.Interfaces;
using QuizApp.Shared.DTO;
using QuizApp.Shared.Models;

namespace QuizApp.Client.Pages.TestPages;

public class TestingPageBase : ComponentBase
{
    [Parameter]
    public Guid Id { get; set; }

    [Inject]
    public ITestService testService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [CascadingParameter]
    protected Task<AuthenticationState> AuthenticationState { get; set; }

    public Test? Test { get; set; }

    public List<QuestionForTestParticipantDTO>? questionForTestParticipantDTOs { get; set; }

public TestResultDTO testResultDTO { get; set; }

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
            questionForTestParticipantDTOs = await testService.StartTest(Id);
            Test = await testService.GetTestByIdForUser(Id);
        }
    }

    protected async Task FinishTest()
    {
        testResultDTO = await testService.FinishTest(Id, questionForTestParticipantDTOs!);
        NavigationManager.NavigateTo($"/testinfo/{Id}");
    }
}
