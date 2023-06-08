using Microsoft.EntityFrameworkCore;
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

    public async Task<List<TestParticipant>> GetTestParticipantByTestIdByGroupIdAsync(Guid testId, Guid groupId)
    {
        return await _context.TestParticipants
            .Include(x => x.User)
            .Where(x => x.TestId == testId && x.User!.GroupId == groupId)
            .ToListAsync();
    }

    public async Task<TestParticipant?> GetTestParticipantByTestIdAsync(Guid testId)
    {
        return await _context.TestParticipants
            .Include(x => x.Test)
            .FirstOrDefaultAsync(x => x.TestId == testId);
    }

    public async Task<bool> IsTestParticipantExistAsync(Guid id)
    {
        var testParticipant = await _context.TestParticipants
            .Where(x => x.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return testParticipant != null;
    }

    public async Task<bool> AddTestParticipantsByGroupIdAsync(Guid testId, Guid groupId)
    {
        var testInDB = await _context.Tests.AsNoTracking().FirstOrDefaultAsync(x => x.Id == testId);
        var groupInDB = await _context.Groups.AsNoTracking().FirstOrDefaultAsync(x => x.Id == groupId);

        if (testInDB == null || groupInDB == null)
        {
            return false;
        }
        var users = await _context.Users
            .Where(x => x.GroupId == groupId)
            .Select(y => new TestParticipant
            {
                Id = Guid.NewGuid(),
                TestId = testId,
                UserId = y.Id
            })
            .ToArrayAsync();

        await _context.TestParticipants.AddRangeAsync(users);
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

    public async Task<bool> DeleteTestParticipantsByGroupIdAsync(Guid testId, Guid groupId)
    {
        var testInDB = await _context.Tests.AsNoTracking().FirstOrDefaultAsync(x => x.Id == testId);
        var groupInDB = await _context.Groups.AsNoTracking().FirstOrDefaultAsync(x => x.Id == groupId);

        if (testInDB == null || groupInDB == null)
        {
            return false;
        }
        var testParticipants = await _context.TestParticipants
            .Where(x => x.TestId == testId)
            .Include(x => x.User)
            .Where(x => x.User!.GroupId == groupId)
            .ToArrayAsync();

        _context.TestParticipants.RemoveRange(testParticipants);
        var result = await _context.SaveChangesAsync();

        return result > 0;
    }
}
