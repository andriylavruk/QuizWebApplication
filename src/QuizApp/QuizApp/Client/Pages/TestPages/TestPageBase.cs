using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using QuizApp.Client.Services.Interfaces;
using QuizApp.Shared.Models;

namespace QuizApp.Client.Pages.TestPages;

public class TestPageBase : ComponentBase
{
    [Parameter]
    public Guid Id { get; set; }

    [Inject]
    public ITestService testService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    public string btnText = string.Empty;

    public Test test = new Test();

    [CascadingParameter]
    protected Task<AuthenticationState> AuthenticationState { get; set; }

    protected bool _signInSuccessful = false;

    public string ErrorMessage { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthenticationState).User;

        btnText = Id == Guid.Empty ? "Створити тест" : "Редагувати тест";

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
            test = await testService.GetTestById(Id);
        }
    }

    protected async Task HandleSubmit()
    {
        if (Id == Guid.Empty)
        {
            await testService.CreateTest(test);
        }
        else
        {
            await testService.UpdateTest(test);
        }

        NavigationManager.NavigateTo("/tests");
    }

    protected async Task DeleteGroup()
    {
        await testService.DeleteTest(test.Id);
        NavigationManager.NavigateTo("/tests");
    }
}
