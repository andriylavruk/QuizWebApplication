using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QuizApp.Server.Repositories.Interfaces;
using QuizApp.Shared.DTO;
using QuizApp.Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuizApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public AuthController(IConfiguration configuration,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IMapper mapper)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult> RegisterAsync(RegisterUserDTO registerUserDTO)
    {
        var oldUser = await _userRepository.GetUserByEmailAsync(registerUserDTO.Email);
        var defaultRole = await _roleRepository.GetRoleByNameAsync("Student");

        if (oldUser != null)
        {
            return BadRequest("This mail is already taken.");
        }

        var user = _mapper.Map<User>(registerUserDTO);
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerUserDTO.Password);

        user.Role = defaultRole;
        user.PasswordHash = passwordHash;

        var result = await _userRepository.CreateUserAsync(user);

        if (!result)
        {
            return BadRequest("Wrong input data.");
        }

        return Ok();
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<string?>> Login(LoginUserDTO loginUserDTO)
    {
        var user = await _userRepository.GetUserByEmailAsync(loginUserDTO.Email);

        if (user == null)
        {
            return BadRequest("User not found");
        }

        if (!BCrypt.Net.BCrypt.Verify(loginUserDTO.Password, user.PasswordHash))
        {
            return BadRequest("Wrong password.");
        }

        var token = CreateToken(user);

        return Ok(token);
    }

    [NonAction]
    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim (ClaimTypes.Role, user.Role!.Name),
            new Claim (ClaimTypes.Name, user.Email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretKey").Value!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}
