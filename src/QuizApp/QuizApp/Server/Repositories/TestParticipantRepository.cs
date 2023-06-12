using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuizApp.Server.Data;
using QuizApp.Server.Repositories.Interfaces;
using QuizApp.Shared.Models;

namespace QuizApp.Server.Repositories;

public class TestParticipantRepository : ITestParticipantRepository
{
    private readonly DataContext _context;

    public TestParticipantRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<TestParticipant>> GetTestParticipantsByGroupIdAsunc(Guid groupId)
    {
        return await _context.TestParticipants
            .Include(x => x.User)
            .Where(x => x.User!.GroupId == groupId)
            .ToListAsync();
    }

    public async Task<TestParticipant?> GetTestParticipantByIdAsync(Guid id)
    {
        return await _context.TestParticipants.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<TestParticipant>> GetTestParticipantsByTestIdByGroupIdAsync(Guid testId, Guid groupId)
    {
        return await _context.TestParticipants
            .Include(x => x.User)
            .Where(x => x.TestId == testId && x.User!.GroupId == groupId)
            .OrderBy(x => x.User!.LastName)
            .ThenBy(x => x.User!.FirstName)
            .ToListAsync();
    }

    public async Task<TestParticipant?> GetTestParticipantByTestIdByUserIdAsync(Guid testId, Guid userId)
    {
        return await _context.TestParticipants
            .Include(x => x.Test)
            .FirstOrDefaultAsync(x => x.TestId == testId && x.UserId == userId);
    }

    public async Task<IEnumerable<TestParticipant>> GetTestParticipantsByGroupIdByUserIdAsync(Guid groupId, Guid userId)
    {
        var group = await _context.Groups.AsNoTracking().FirstOrDefaultAsync(x => x.Id == groupId);
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);

        if (group == null || user == null || user.GroupId == null)
        {
            return Enumerable.Empty<TestParticipant>();
        }

        var tests =
            from test in _context.Tests
            join testprtc in _context.TestParticipants on test.Id equals testprtc.TestId
            join grp in _context.Groups on testprtc.User!.GroupId equals grp.Id
            where grp.Id == groupId
            group test by test.Id into t
            select new Test
            {
                Id = t.Key,
                Name = t.FirstOrDefault()!.Name
            };

        var testsList = await tests.ToListAsync();

        if (testsList.Count == 0)
        {
            return Enumerable.Empty<TestParticipant>();
        }

        var testParticipants = testsList.Select(x => new TestParticipant
        {
            Id = Guid.NewGuid(),
            TestId = x.Id,
            UserId = user.Id
        });

        return testParticipants;
    }

    public async Task<bool> IsTestParticipantExistAsync(Guid id)
    {
        var testParticipant = await _context.TestParticipants
            .Where(x => x.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return testParticipant != null;
    }

    public async Task<bool> AddTestParticipantByGroupIdByUserIdAsync(Guid groupId, Guid userId)
    {
        var testParticipants = await GetTestParticipantsByGroupIdByUserIdAsync(groupId, userId);

        if(testParticipants.IsNullOrEmpty())
        {
            return false;
        }

        await _context.TestParticipants.AddRangeAsync(testParticipants);
        var result = await _context.SaveChangesAsync();

        return result > 0;
    }

    public async Task<bool> AddTestParticipantsByTestIdByGroupIdAsync(Guid testId, Guid groupId)
    {
        var testInDB = await _context.Tests.AsNoTracking().FirstOrDefaultAsync(x => x.Id == testId);
        var groupInDB = await _context.Groups.AsNoTracking().FirstOrDefaultAsync(x => x.Id == groupId);

        if (testInDB == null || groupInDB == null)
        {
            return false;
        }

        var testParticipants = await _context.Users
            .Where(x => x.GroupId == groupId)
            .Select(y => new TestParticipant
            {
                Id = Guid.NewGuid(),
                TestId = testId,
                UserId = y.Id
            })
            .ToArrayAsync();

        await _context.TestParticipants.AddRangeAsync(testParticipants);
        var result = await _context.SaveChangesAsync();

        return result > 0;
    }

    public async Task<bool> UpdateTestParticipantAsync(TestParticipant testParticipant, Guid currentUserId)
    {
        if (testParticipant == null)
        {
            return false;
        }

        if(testParticipant.UserId == currentUserId)
        {
            _context.Update(testParticipant);
            await _context.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task<bool> DeleteTestParticipantByGroupIdByUserIdAsync(Guid groupId, Guid userId)
    {
        var group = await _context.Groups.AsNoTracking().FirstOrDefaultAsync(x => x.Id == groupId);
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);


        if (group == null || user == null)
        {
            return false;
        }

        var testParticipants = await _context.TestParticipants
            .Include(x => x.User)
            .Where(x => x.UserId == userId && x.User!.GroupId == groupId)
            .ToListAsync();

        if (testParticipants.Count == 0)
        {
            return false;
        }

        _context.TestParticipants.RemoveRange(testParticipants);
        var result = await _context.SaveChangesAsync();

        return result > 0;
    }

    public async Task<bool> DeleteTestParticipantsByTestIdByGroupIdAsync(Guid testId, Guid groupId)
    {
        var testInDB = await _context.Tests.AsNoTracking().FirstOrDefaultAsync(x => x.Id == testId);
        var groupInDB = await _context.Groups.AsNoTracking().FirstOrDefaultAsync(x => x.Id == groupId);

        if (testInDB == null || groupInDB == null)
        {
            return false;
        }
        var testParticipants = await _context.TestParticipants
            .Include(x => x.User)
            .Where(x => x.TestId == testId && x.User!.GroupId == groupId)
            .ToArrayAsync();

        _context.TestParticipants.RemoveRange(testParticipants);
        var result = await _context.SaveChangesAsync();

        return result > 0;
    }
}
