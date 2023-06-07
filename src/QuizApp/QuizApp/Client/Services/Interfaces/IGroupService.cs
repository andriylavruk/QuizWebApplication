using QuizApp.Shared.Models;

namespace QuizApp.Client.Services.Interfaces;

public interface IGroupService
{
    List<Group> Groups { get; set; }

    Task<List<Group>> GetAllGroups();
    Task<Group> GetGroupById(Guid id);
    Task<List<Group>> GetGroupsByTestId(Guid testId);
    Task<List<Group>> GetGroupsToAddByTestId(Guid testId);
    Task CreateGroup(Group group);
    Task UpdateGroup(Group group);
    Task DeleteGroup(Guid id);
    Task SetUserGroup(Guid userId, Guid groupId);
    Task UnsetUserGroup(Guid userId);
}
