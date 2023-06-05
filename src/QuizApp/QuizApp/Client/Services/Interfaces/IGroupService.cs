using QuizApp.Shared.Models;

namespace QuizApp.Client.Services.Interfaces;

public interface IGroupService
{
    List<Group> Groups { get; set; }

    Task<List<Group>> GetAllGroups();
    Task<Group> GetGroupById(Guid id);
    Task CreateGroup(Group group);
    Task UpdateGroup(Group group);
    Task DeleteGroup(Guid id);
    Task SetUserGroup(Guid userId, Guid groupId);
}
