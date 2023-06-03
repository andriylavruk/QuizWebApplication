using Microsoft.EntityFrameworkCore;
using QuizApp.Server.Data;
using QuizApp.Server.Repositories.Interfaces;
using QuizApp.Shared.Models;

namespace QuizApp.Server.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly DataContext _context;

    public RoleRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Role>> GetAllRolesAsync()
    {
        return await _context.Roles.ToListAsync();
    }

    public async Task<Role?> GetRoleByIdAsync(Guid id)
    {
        return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Role?> GetRoleByNameAsync(string roleName)
    {
        return await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
    }

    public async Task<bool> CreateRoleAsync(Role role)
    {
        if (role != null)
        {
            var oldRole = await GetRoleByIdAsync(role.Id);

            if (oldRole != null)
            {
                return false;
            }

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> UpdateAsync(Role role)
    {
        if (role == null)
        {
            return false;
        }

        _context.Update(role);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Role role)
    {
        if (role != null)
        {
            _context.Remove(role!);
            await _context.SaveChangesAsync();

            return true;
        }
        else
        {
            return false;
        }
    }
}
