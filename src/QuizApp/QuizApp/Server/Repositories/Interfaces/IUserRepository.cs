using QuizApp.Shared.Models;

namespace QuizApp.Server.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(Guid id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> IsActivatedUserAsync(Guid id);
    Task<bool> ActivateUserAsync(Guid id);
    Task<bool> DeActivateUserAsync(Guid id);
    Task<bool> SetUserRole(User user, Role role);
    Task<bool> IsUserExistAsync(Guid id);
    Task<bool> CreateUserAsync(User user);
    Task<bool> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(User user);
}
