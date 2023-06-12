using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Server.Repositories.Interfaces;
using QuizApp.Shared.DTO;
using QuizApp.Shared.Models;

namespace QuizApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TestController : ControllerBase
{
    private readonly ITestRepository _testRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITestParticipantRepository _testParticipantRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IMapper _mapper;

    public TestController(ITestRepository testRepository,
        IUserRepository userRepository,
        ITestParticipantRepository testParticipantRepository,
        IQuestionRepository questionRepository,
        IMapper mapper)
    {
        _testRepository = testRepository;
        _userRepository = userRepository;
        _testParticipantRepository = testParticipantRepository;
        _questionRepository = questionRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<List<Test>>> GetAllTests()
    {
        var tests = await _testRepository.GetAllTestsAsync();

        return Ok(tests);
    }

    [HttpGet]
    [Route("/testsForUser/")]
    [Authorize(Roles = "Student")]
    public async Task<ActionResult<List<Test>>> GetTestsForUser()
    {
        var currentUserId = _userRepository.GetCurrentUserId();

        var tests = await _testRepository.GetTestsForUserAsync(currentUserId);

        return Ok(tests);
    }

    [HttpGet]
    [Route("/startTest/{testId:guid}")]
    [Authorize(Roles = "Student")]
    public async Task<ActionResult<List<QuestionForTestParticipantDTO>>> StartTest(Guid testId)
    {
        var currentUserId = _userRepository.GetCurrentUserId();
        var test = await _testRepository.GetTestByIdAsync(testId);

        if (test == null)
        {
            return BadRequest();
        }

        var participant = await _testParticipantRepository.GetTestParticipantByTestIdByUserIdAsync(testId, currentUserId);
        var questionsForTestParticipant = await _questionRepository.GetQuestionsForTestParticipantAsync(participant!.Id);

        if (participant == null || test.Id != participant.TestId)
        {
            return BadRequest("This user does not have access to this test.");
        }

        if (participant.FinishedAt != null)
        {
            return BadRequest("The test has already been passed.");
        }

        if(participant.StartedAt == null)
        {
            participant.StartedAt = DateTime.Now;
        }

        await _testParticipantRepository.UpdateTestParticipantAsync(participant, currentUserId);

        return Ok(questionsForTestParticipant);
    }

    [HttpPost]
    [Route("/finishTest/{testId:guid}")]
    [Authorize(Roles = "Student")]
    public async Task<ActionResult<TestResultDTO>> FinishTest(Guid testId, List<QuestionForTestParticipantDTO> questionForTestParticipantDTOs)
    {
        var currentUserId = _userRepository.GetCurrentUserId();
        var test = await _testRepository.GetTestByIdAsync(testId);

        if (test == null)
        {
            return BadRequest();
        }

        var participant = await _testParticipantRepository.GetTestParticipantByTestIdByUserIdAsync(testId, currentUserId);

        if (participant == null || test.Id != participant.TestId)
        {
            return BadRequest("This user does not have access to this test.");
        }

        if (participant.StartedAt == null)
        {
            return BadRequest("The test has not yet started.");
        }

        if(participant.FinishedAt != null)
        {
            return BadRequest("The test has already been completed.");
        }

        var grade = await _questionRepository.CalculateGrade(test.Id, questionForTestParticipantDTOs);

        participant.FinishedAt = DateTime.Now;
        participant.Grade = grade;

        await _testParticipantRepository.UpdateTestParticipantAsync(participant, currentUserId);

        var testResult = new TestResultDTO
        {
            User = _mapper.Map<UserDTO>(await _userRepository.GetUserByIdAsync(currentUserId)),
            Test = test,
            Grade = grade,
            NumberOfQuestions = questionForTestParticipantDTOs.Count
        };

        return Ok(testResult);
    }

    [HttpGet]
    [Route("{id:guid}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetTestById(Guid id)
    {
        var test = await _testRepository.GetTestByIdAsync(id);

        if (test == null)
        {
            return NotFound();
        }

        return Ok(test);
    }

    [HttpGet]
    [Route("/testForUser/{id:guid}")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetTestByIdForUser(Guid id)
    {
        var currentUserId = _userRepository.GetCurrentUserId();

        var tests = await _testRepository.GetTestsForUserAsync(currentUserId);
        var test = tests.FirstOrDefault(x => x.Id == id);

        if (test == null)
        {
            return NotFound();
        }

        return Ok(test);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> CreateTest(Test test)
    {
        var result = false;

        if (test != null)
        {
            result = await _testRepository.CreateTestAsync(test);
        }

        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPut]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateTest(Test test)
    {
        var isTestExist = await _testRepository.IsTestExistAsync(test.Id);

        if (!isTestExist)
        {
            return NotFound();
        }

        await _testRepository.UpdateTestAsync(test);

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteTest(Guid id)
    {
        var test = await _testRepository.GetTestByIdAsync(id);

        if (test == null)
        {
            return NotFound();
        }

        var deleted = await _testRepository.DeleteTestAsync(test);

        if (deleted)
        {
            return NoContent();
        }

        return NotFound();
    }
}
