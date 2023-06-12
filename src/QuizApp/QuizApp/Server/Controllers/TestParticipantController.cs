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
    [Authorize(Roles = "Administrator")]
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
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetTestParticipantById(Guid id)
    {
        var testParticipant = await _testParticipantRepository.GetTestParticipantByIdAsync(id);

        if (testParticipant == null)
        {
            return NotFound();
        }

        return Ok(testParticipant);
    }

    [HttpGet]
    [Route("participants/{testId:guid}/{groupId:guid}")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<List<TestParticipant>>> GetTestParticipantsByTestIdByGroupIdAsync(Guid testId, Guid groupId)
    {
        var testParticipants = await _testParticipantRepository.GetTestParticipantsByTestIdByGroupIdAsync(testId, groupId);

        return Ok(testParticipants);
    }

    [HttpGet]
    [Route("/participantforuser/{testId:guid}")]
    [Authorize(Roles = "Student")]
    public async Task<TestParticipantInformationDTO?> GetTestParticipantInfoByTestIdForUser(Guid testId)
    {
        var currentUser = _userRepository.GetCurrentUserId();
        var participantInfo = await _testParticipantRepository.GetTestParticipantByTestIdByUserIdAsync(testId, currentUser);

        return _mapper.Map<TestParticipantInformationDTO>(participantInfo);
    }

    [HttpPost("addtestparticipants/{testId:guid}/{groupId:guid}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AddTestParticipantsByGroupId(Guid testId, Guid groupId)
    {
        var test = await _testRepository.GetTestByIdAsync(testId);
        var group = await _groupRepository.GetGroupByIdAsync(groupId);

        if (test == null || group == null)
        {
            return BadRequest();
        }

        await _testParticipantRepository.AddTestParticipantsByTestIdByGroupIdAsync(testId, groupId);

        return Ok();
    }

    [HttpDelete("deletetestparticipants/{testId:guid}/{groupId:guid}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteTestParticipantsByGroupId(Guid testId, Guid groupId)
    {
        var test = await _testRepository.GetTestByIdAsync(testId);
        var group = await _groupRepository.GetGroupByIdAsync(groupId);

        if (test == null || group == null)
        {
            return BadRequest();
        }

        await _testParticipantRepository.DeleteTestParticipantsByTestIdByGroupIdAsync(testId, groupId);

        return NoContent();
    }

    [HttpPost("{groupId:guid}/{userId:guid}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AddTestParticipant(Guid groupId, Guid userId)
    {

        var group = await _groupRepository.GetGroupByIdAsync(groupId);
        var user = await _userRepository.GetUserByIdAsync(userId);

        if (group == null || user == null)
        {
            return NotFound();
        }

        await _testParticipantRepository.AddTestParticipantByGroupIdByUserIdAsync(groupId, userId);

        return Ok();
    }

    [HttpPut]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateTestParticipant(TestParticipantInformationDTO testParticipantDTO)
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

    [HttpDelete("{groupId:guid}/{userId:guid}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteTestParticipant(Guid groupId, Guid userId)
    {
        var isGroupExist = await _groupRepository.IsGroupExistAsync(groupId);
        var isUserExist = await _userRepository.IsUserExistAsync(userId);

        if (isGroupExist == false || isUserExist == false)
        {
            return NotFound();
        }

        var deleted = await _testParticipantRepository.DeleteTestParticipantByGroupIdByUserIdAsync(groupId, userId);

        if (deleted)
        {
            return NoContent();
        }

        return NotFound();
    }
}
