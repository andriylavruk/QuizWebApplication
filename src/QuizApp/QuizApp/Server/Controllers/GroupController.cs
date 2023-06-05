using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Server.Repositories.Interfaces;
using QuizApp.Shared.Models;

namespace QuizApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Administrator")]
public class GroupController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IGroupRepository _groupRepository;

    public GroupController(IUserRepository userRepository, IGroupRepository groupRepository)
    {
        _userRepository = userRepository;
        _groupRepository = groupRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Group>>> GetAllGroups()
    {
        var groups = await _groupRepository.GetAllGroupsAsync();

        return Ok(groups);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetGroupById(Guid id)
    {
        var group = await _groupRepository.GetGroupByIdAsync(id);

        if (group == null)
        {
            return NotFound();
        }

        return Ok(group);
    }

    [HttpPost]
    public async Task<IActionResult> GreateGroup(Group group)
    {
        var result = false;

        if (group != null)
        {
            result = await _groupRepository.CreateGroupAsync(group);
        }

        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateGroup(Group group)
    {
        var isGroupExist = await _groupRepository.IsGroupExistAsync(group.Id);

        if (!isGroupExist)
        {
            return NotFound();
        }

        await _groupRepository.UpdateGroupAsync(group);

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteGroup(Guid id)
    {
        var group = await _groupRepository.GetGroupByIdAsync(id);

        if (group == null)
        {
            return NotFound();
        }

        var deleted = await _groupRepository.DeleteGroupAsync(group);

        if (deleted)
        {
            return NoContent();
        }

        return NotFound();
    }

    [HttpPost("setusergroup/{userId:guid}/{groupId:guid}")]
    public async Task<IActionResult> SetUserGroup(Guid userId, Guid groupId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        var group = await _groupRepository.GetGroupByIdAsync(groupId);

        if (user == null || group == null)
        {
            return NotFound();
        }

        await _groupRepository.SetUserGroup(user, group);

        return Ok();
    }

    [HttpPost("unsetusergroup/{userId:guid}")]
    public async Task<IActionResult> UnsetUserGroup(Guid userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        await _groupRepository.UnsetUserGroup(user);

        return Ok();
    }
}
