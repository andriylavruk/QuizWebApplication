using Microsoft.EntityFrameworkCore;
using QuizApp.Server.Data;
using QuizApp.Server.Repositories.Interfaces;
using QuizApp.Shared.Models;
using System.Collections;
using System.Collections.Generic;

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
        return await _context.Groups
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<List<Group>> GetGroupsByTestIdAsync(Guid testId)
    {
        var participants = _context.TestParticipants
            .Include(x => x.User)
            .Where(x => x.TestId == testId);

        var groups =
            from grp in _context.Groups
            join partc in participants on grp.Id equals partc.User!.GroupId
            group grp by grp.Id into g
            select new Group
            {
                Id = g.Key,
                Name = g.FirstOrDefault()!.Name
            };

        return await groups
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<Group?> GetGroupByIdAsync(Guid id)
    {
        return await _context.Groups.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Group?> GetGroupByNameAsync(string groupName)
    {
        return await _context.Groups.FirstOrDefaultAsync(r => r.Name == groupName);
    }

    public async Task<List<Group>> GetGroupsToAddByTestIdAsync(Guid testId)
    {
        var groups =
            from grp in _context.Groups
            join users in _context.Users on grp.Id equals users.GroupId
            group grp by grp.Id into g
            select new Group
            {
                Id = g.Key,
                Name = g.FirstOrDefault()!.Name
            };

        var groupList = await groups.OrderBy(x => x.Name).ToListAsync();
        var groupsInTest = await GetGroupsByTestIdAsync(testId);

        groupList.RemoveAll(x => groupsInTest.Any(y => y.Id == x.Id));

        return groupList;
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

    public async Task<bool> UnsetUserGroup(User user)
    {
        var userInDB = await _context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);

        if (user == null || userInDB == null)
        {
            return false;
        }

        userInDB.Group = null;
        userInDB.GroupId = null;
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
