using QuizApp.Shared.Models;

namespace QuizApp.Server.Repositories.Interfaces;

public interface IGroupRepository
{
    Task<List<Group>> GetAllGroupsAsync();
    Task<Group?> GetGroupByIdAsync(Guid groupId);
    Task<Group?> GetGroupByNameAsync(string groupName);
    Task<bool> IsGroupExistAsync(Guid id);
    Task<bool> SetUserGroup(User user, Group group);
    Task<bool> UnsetUserGroup(User user);
    Task<bool> CreateGroupAsync(Group group);
    Task<bool> UpdateGroupAsync(Group group);
    Task<bool> DeleteGroupAsync(Group group);
}
