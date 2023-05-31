using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Server.Repositories.Interfaces;
using QuizApp.Shared.DTO;
using QuizApp.Shared.Models;

namespace QuizApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize(Roles = "Administrator")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public UserController(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAllUsers()
    {
        var users = await _userRepository.GetAllUsersAsync();
        var result = _mapper.Map<List<UserDTO>>(users);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<User>> GetUserById(Guid id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        var mappedUser = _mapper.Map<UserDTO>(user);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(mappedUser);
    }

    [HttpGet("role")]
    public async Task<ActionResult<List<Role>>> GetAllRoles()
    {
        var roles = await _roleRepository.GetAllRolesAsync();

        return Ok(roles);
    }

    [HttpGet("role/{id:guid}")]
    public async Task<ActionResult<Role>> GetRoleById(Guid id)
    {
        var role = await _roleRepository.GetRoleByIdAsync(id);

        if (role == null)
        {
            return NotFound();
        }

        return Ok(role);
    }

    [HttpPost("setrole/{userId:guid}/{roleId:guid}")]
    public async Task<IActionResult> SetUserRole(Guid userId, Guid roleId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        var role = await _roleRepository.GetRoleByIdAsync(roleId);

        if (user == null || role == null)
        {
            return NotFound();
        }

        await _userRepository.SetUserRole(user, role);

        return Ok();
    }
}
