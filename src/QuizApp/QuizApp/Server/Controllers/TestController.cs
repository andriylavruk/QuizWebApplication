using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Server.Repositories.Interfaces;
using QuizApp.Shared.Models;

namespace QuizApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TestController : ControllerBase
{
    private readonly ITestRepository _testRepository;
    private readonly IUserRepository _userRepository;

    public TestController(ITestRepository testRepository, IUserRepository userRepository)
    {
        _testRepository = testRepository;
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Test>>> GetAllTests()
    {
        var tests = await _testRepository.GetAllTestsAsync();

        return Ok(tests);
    }

    [HttpGet]
    [Route("/testsForUser")]
    [Authorize(Roles = "Student")]
    public async Task<ActionResult<List<Test>>> GetTestsForTestUser()
    {
        var currentUserId = _userRepository.GetCurrentUserId();

        var tests = await _testRepository.GetTestsForUserAsync(currentUserId);

        return Ok(tests);
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
