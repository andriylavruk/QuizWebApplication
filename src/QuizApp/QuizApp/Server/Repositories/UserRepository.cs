using Microsoft.EntityFrameworkCore;
using QuizApp.Server.Data;
using QuizApp.Server.Repositories.Interfaces;
using QuizApp.Shared.Models;
using System.Security.Claims;

namespace QuizApp.Server.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserRepository(DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users
            .Include(x => x.Role)
            .Include(x => x.Group)
            .OrderBy(x => x.LastName)
            .ThenBy(x => x.FirstName)
            .ToListAsync();
    }

    public async Task<List<User>> GetUsersWithoutGroupAsync()
    {
        return await _context.Users
            .Include(x => x.Role)
            .Include(x => x.Group)
            .Where(x => x.GroupId == null && x.Role!.Name == "Student")
            .OrderBy(x => x.LastName)
            .ThenBy(x => x.FirstName)
            .ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _context.Users
            .Include(x => x.Role)
            .Include(x => x.Group)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<User>> GetUsersByGroupIdAsync(Guid groupId)
    {
        return await _context.Users
            .Where(x => x.GroupId == groupId)
            .OrderBy(x => x.LastName)
            .ThenBy(x => x.FirstName)
            .ToListAsync();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> SetUserRole(User user, Role role)
    {
        var userInDB = await GetUserByIdAsync(user.Id);
        var roleInDB = await _context.Roles.FirstOrDefaultAsync(x => x.Id == role.Id);

        if (user == null || role == null || userInDB == null || roleInDB == null)
        {
            return false;
        }

        userInDB.Role = roleInDB;
        var result = await _context.SaveChangesAsync();

        return result > 0;
    }

    public async Task<bool> IsUserExistAsync(Guid id)
    {
        var users = _context.Users.Where(x => x.Id == id).AsNoTracking();
        var user = await users.FirstOrDefaultAsync(x => x.Id == id);

        return user != null;
    }

    public async Task<bool> CreateUserAsync(User user)
    {
        if (user != null)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        if (user == null)
        {
            return false;
        }

        _context.Update(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteUserAsync(User user)
    {
        if (user != null)
        {
            var userDb = await GetUserByEmailAsync(user.Email);

            _context.Remove(userDb!);
            await _context.SaveChangesAsync();

            return true;
        }
        else
        {
            return false;
        }
    }

    public Guid GetCurrentUserId()
    {
        var id = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (id == null)
        {
            return Guid.Empty;
        }

        return new Guid(id);
    }
}
