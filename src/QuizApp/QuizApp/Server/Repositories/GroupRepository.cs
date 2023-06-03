using Microsoft.EntityFrameworkCore;
using QuizApp.Server.Data;
using QuizApp.Server.Repositories.Interfaces;
using QuizApp.Shared.Models;

namespace QuizApp.Server.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly DataContext _context;

    public GroupRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Group>> GetAllGroupsAsync()
    {
        return await _context.Groups.ToListAsync();
    }

    public async Task<Group?> GetGroupByIdAsync(Guid id)
    {
        return await _context.Groups.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Group?> GetGroupByNameAsync(string groupName)
    {
        return await _context.Groups.FirstOrDefaultAsync(r => r.Name == groupName);
    }

    public async Task<bool> IsGroupExistAsync(Guid id)
    {
        var groups = _context.Groups.Where(x => x.Id == id).AsNoTracking();
        var group = await groups.FirstOrDefaultAsync(x => x.Id == id);

        return group != null;
    }

    public async Task<bool> SetUserGroup(User user, Group group)
    {
        var userInDB = await _context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
        var groupInDB = await _context.Groups.FirstOrDefaultAsync(x => x.Id == group.Id);
        var userGroup = userInDB?.Group;

        if (user == null || group == null || userInDB == null || groupInDB == null)
        {
            return false;
        }

        userInDB.Group = groupInDB;
        userInDB.GroupId = groupInDB.Id;
        var result = await _context.SaveChangesAsync();

        return result > 0;
    }

    public async Task<bool> CreateGroupAsync(Group group)
    {
        if (group != null)
        {
            var oldGroup = await GetGroupByIdAsync(group.Id);

            if (oldGroup != null)
            {
                return false;
            }

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> UpdateGroupAsync(Group group)
    {
        if (group == null)
        {
            return false;
        }

        _context.Update(group);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteGroupAsync(Group group)
    {
        if (group != null)
        {
            _context.Remove(group!);
            await _context.SaveChangesAsync();

            return true;
        }
        else
        {
            return false;
        }
    }
}
