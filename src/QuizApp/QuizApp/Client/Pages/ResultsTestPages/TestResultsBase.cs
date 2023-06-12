using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using QuizApp.Client.Services.Interfaces;
using QuizApp.Shared.Models;

namespace QuizApp.Client.Pages.ResultsTestPages;

public class TestResultsBase : ComponentBase
{
    [Parameter]
    public Guid testId { get; set; }

    [Parameter]
    public Guid groupId { get; set; }

    [Inject]
    public ITestParticipantService testParticipantService { get; set; }

    [Inject]
    public IQuestionService questionService { get; set; }

    [Inject]
    public ITestService testService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    public Test? Test { get; set; }

    private List<Question> testQuestions { get; set; }

    public int numberOfQuestions { get; set; }

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
                await testParticipantService.GetTestParticipantsByTestIdByGroupId(testId, groupId);
                testQuestions = await questionService.GetQuestionsByTestId(testId);
                numberOfQuestions = testQuestions.Count;
                Test = await testService.GetTestById(testId);
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
}
