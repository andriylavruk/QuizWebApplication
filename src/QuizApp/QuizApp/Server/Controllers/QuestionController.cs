using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Server.Repositories;
using QuizApp.Server.Repositories.Interfaces;
using QuizApp.Shared.Models;

namespace QuizApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class QuestionController : ControllerBase
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITestParticipantRepository _testParticipantRepository;

    public QuestionController(IQuestionRepository questionRepository, 
        IUserRepository userRepository, 
        ITestParticipantRepository testParticipantRepository)
    {
        _questionRepository = questionRepository; ;
        _userRepository = userRepository;
        _testParticipantRepository = testParticipantRepository;
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<List<Question>>> GetAllQuestions()
    {
        var questions = await _questionRepository.GetAllQuestionsAsync();

        return Ok(questions);
    }

    [HttpGet]
    [Route("/questionsForTestParticipant/{testId:guid}")]
    [Authorize(Roles = "Student")]
    public async Task<ActionResult<List<Question>>> GetQuestionsForTestParticipantByTestId(Guid testId)
    {
        var currentUserId = _userRepository.GetCurrentUserId();

        var participant = await _testParticipantRepository.GetTestParticipantByTestIdAsync(testId);

        if (participant!.UserId != currentUserId)
        {
            return BadRequest("This user does not have access to these questions.");
        }

        var questionsForTestParticipant = await _questionRepository.GetQuestionsForTestParticipantAsync(participant!.Id);

        return Ok(questionsForTestParticipant);
    }

    [HttpGet]
    [Route("{id:guid}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetQuestionById(Guid id)
    {
        var question = await _questionRepository.GetQuestionByIdAsync(id);

        if (question == null)
        {
            return NotFound();
        }

        return Ok(question);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GreateQuestion(Question question)
    {
        var result = false;

        if (question != null)
        {
            result = await _questionRepository.CreateQuestionAsync(question);
        }

        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPut]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateQuestion(Question question)
    {
        var isQuestionExist = await _questionRepository.IsQuestionExistAsync(question.Id);

        if (!isQuestionExist)
        {
            return NotFound();
        }

        await _questionRepository.UpdateQuestionAsync(question);

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteTest(Guid id)
    {
        var question = await _questionRepository.GetQuestionByIdAsync(id);

        if (question == null)
        {
            return NotFound();
        }

        var deleted = await _questionRepository.DeleteQuestionAsync(question);

        if (deleted)
        {
            return NoContent();
        }

        return NotFound();
    }
}
