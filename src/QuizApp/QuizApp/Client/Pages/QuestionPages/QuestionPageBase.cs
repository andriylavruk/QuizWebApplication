using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using QuizApp.Client.Services.Interfaces;
using QuizApp.Shared.Models;

namespace QuizApp.Client.Pages.QuestionPages;

public class QuestionPageBase : ComponentBase
{
    [Parameter]
    public Guid testId { get; set; }

    [Parameter]
    public Guid questionId { get; set; }

    [Inject]
    public IQuestionService questionService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    public string btnText = string.Empty;

    public Question question = new Question();

    [CascadingParameter]
    protected Task<AuthenticationState> AuthenticationState { get; set; }

    protected bool _signInSuccessful = false;

    public string ErrorMessage { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthenticationState).User;

        btnText = questionId == Guid.Empty ? "Створити питання" : "Редагувати питаннясс";

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
        if (questionId != Guid.Empty)
        {
            question = await questionService.GetQuestionById(questionId);
        }
    }

    protected async Task HandleSubmit()
    {
        if (questionId == Guid.Empty)
        {
            question.TestId = testId;
            await questionService.CreateQuestion(question);
        }
        else
        {
            await questionService.UpdateQuestion(question);
        }

        NavigationManager.NavigateTo($"/testquestions/{question.TestId}");
    }

    protected async Task DeleteQuestion()
    {
        await questionService.DeleteQuestion(questionId);
        NavigationManager.NavigateTo($"/testquestions/{question.TestId}");
    }
}
