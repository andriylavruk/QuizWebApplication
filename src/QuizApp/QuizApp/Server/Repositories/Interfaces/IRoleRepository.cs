using QuizApp.Shared.Models;

namespace QuizApp.Server.Repositories.Interfaces;

public interface IRoleRepository
{
    Task<List<Role>> GetAllRolesAsync();
    Task<Role?> GetRoleByIdAsync(Guid roleId);
    Task<Role?> GetRoleByNameAsync(string roleName);
    Task<bool> CreateRoleAsync(Role role);
    Task<bool> UpdateAsync(Role role);
    Task<bool> DeleteAsync(Role role);
}
