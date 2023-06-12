using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using QuizApp.Client.Pages.QuestionPages;
using QuizApp.Client.Services;
using QuizApp.Client.Services.Interfaces;
using QuizApp.Shared.DTO;
using QuizApp.Shared.Models;

namespace QuizApp.Client.Pages.TestPages;

public class TestInfoPageBase : ComponentBase
{
    [Parameter]
    public Guid Id { get; set; }

    [Inject]
    public ITestService testService { get; set; }

    [Inject]
    public IQuestionService questionService { get; set; }

    [Inject]
    public ITestParticipantService testParticipantService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    public Test test = new Test();
    public TestParticipantInformationDTO? testInformation { get; set; }
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
            test = await testService.GetTestByIdForUser(Id);
            testInformation = await testParticipantService.GetTestParticipantInfoByTestIdForUser(Id);
            numberOfQuestions = await questionService.GetNumberOfQuestionsByTetsIdForStudent(Id);
        }
    }

    protected void StartTest()
    {
        NavigationManager.NavigateTo($"/testing/{Id}");
    }
}
