using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using QuizApp.Client.Services.Interfaces;
using QuizApp.Shared.Models;

namespace QuizApp.Client.Pages.QuestionPages;

public class TestQuestionsBase : ComponentBase
{
    [Parameter]
    public Guid Id { get; set; }

    [Inject]
    public IQuestionService questionService { get; set; }

    [Inject]
    public ITestService testService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    public Test? Test { get; set; }

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
                await questionService.GetQuestionsByTestId(Id);
                Test = await testService.GetTestById(Id);
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

    protected void EditQuestion(Guid id)
    {
        NavigationManager.NavigateTo($"question/{Id}/{id}");
    }

    protected void CreateQuestion()
    {
        NavigationManager.NavigateTo($"/question/{Id}");
    }

    protected async Task DeleteQuestion(Guid id)
    {
        await questionService.DeleteQuestion(id);
        await questionService.GetQuestionsByTestId(Id);
        NavigationManager.NavigateTo("/questions");
    }
}
