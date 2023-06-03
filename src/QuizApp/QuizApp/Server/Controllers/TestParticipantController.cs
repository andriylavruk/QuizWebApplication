using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Server.Repositories.Interfaces;
using QuizApp.Shared.DTO;
using QuizApp.Shared.Models;

namespace QuizApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Administrator")]
public class TestParticipantController : ControllerBase
{
    private readonly ITestParticipantRepository _testParticipantRepository;
    private readonly ITestRepository _testRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public TestParticipantController(ITestParticipantRepository testParticipantRepository,
        ITestRepository testRepository,
        IGroupRepository groupRepository,
        IUserRepository userRepository,
        IMapper mapper)
    {
        _testParticipantRepository = testParticipantRepository;
        _testRepository = testRepository;
        _groupRepository = groupRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("participants/{groupId:guid}")]
    public async Task<ActionResult<List<TestParticipant>>> GetTestParticipantsByGroupId(Guid groupId)
    {
        var group = await _groupRepository.GetGroupByIdAsync(groupId);

        if (group == null)
        {
            return NotFound();
        }

        return Ok(await _testParticipantRepository.GetTestParticipantsByGroupIdAsunc(groupId));
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetTestParticipantById(Guid id)
    {
        var testParticipant = await _testParticipantRepository.GetTestParticipantByIdAsync(id);

        if (testParticipant == null)
        {
            return NotFound();
        }

        return Ok(testParticipant);
    }

    [HttpPost("addtestparticipants/{testId:guid}/{groupId:guid}")]
    public async Task<IActionResult> AddTestParticipantsByGroup(Guid testId, Guid groupId)
    {
        var test = await _testRepository.GetTestByIdAsync(testId);
        var group = await _groupRepository.GetGroupByIdAsync(groupId);

        if (test == null || group == null)
        {
            return BadRequest();
        }

        await _testParticipantRepository.AddTestParticipantsByGroupIdAsync(testId, groupId);

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTest(TestParticipantInformationDTO testParticipantDTO)
    {
        var currentUserId = _userRepository.GetCurrentUserId();
        var participant = await _testParticipantRepository.GetTestParticipantByIdAsync(testParticipantDTO.Id);

        if (participant == null)
        {
            return NotFound();
        }

        var testParticipant = _mapper.Map(testParticipantDTO, participant);

        await _testParticipantRepository.UpdateTestParticipantAsync(testParticipant, currentUserId);

        return Ok();
    }

    [HttpDelete("{testId:guid}/{groupId:guid}")]
    public async Task<IActionResult> DeleteTestParticipant(Guid testId, Guid groupId)
    {

        var test = await _testRepository.GetTestByIdAsync(testId);
        var group = await _groupRepository.GetGroupByIdAsync(groupId);

        if (test == null || group == null)
        {
            return NotFound();
        }

        await _testParticipantRepository.AddTestParticipantsByGroupIdAsync(testId, groupId);

        var deleted = await _testParticipantRepository.DeleteTestParticipantsByGroupIdAsync(testId, groupId);

        if (deleted)
        {
            return NoContent();
        }

        return NotFound();
    }
}
