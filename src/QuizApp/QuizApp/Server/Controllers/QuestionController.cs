using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    public QuestionController(IQuestionRepository questionRepository, IUserRepository userRepository)
    {
        _questionRepository = questionRepository; ;
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Question>>> GetAllQuestions()
    {
        var questions = await _questionRepository.GetAllQuestionsAsync();

        return Ok(questions);
    }

    [HttpGet]
    [Route("/questionsForTestParticipant")]
    [Authorize(Roles = "Student")]
    public async Task<ActionResult<List<Question>>> GetQuestionsForTestParticipant()
    {
        var currentUserId = _userRepository.GetCurrentUserId();

        var questions = await _questionRepository.GetQuestionsForTestParticipantAsync(currentUserId);

        return Ok(questions);
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
