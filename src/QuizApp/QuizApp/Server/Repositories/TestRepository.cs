using Microsoft.EntityFrameworkCore;
using QuizApp.Server.Data;
using QuizApp.Server.Repositories.Interfaces;
using QuizApp.Shared.Models;

namespace QuizApp.Server.Repositories;

public class TestRepository : ITestRepository
{
    private readonly DataContext _context;

    public TestRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Test>> GetAllTestsAsync()
    {
        return await _context.Tests.ToListAsync();
    }

    public async Task<List<Test>> GetTestsForUserAsync(Guid userId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            return new List<Test>();
        }

        var tests =
            from test in _context.Tests
            join tpar in _context.TestParticipants on test.Id equals tpar.TestId
            where tpar.UserId == user!.Id
            select test;

        return await tests.ToListAsync();
    }

    public async Task<Test?> GetTestByIdAsync(Guid id)
    {
        return await _context.Tests.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<bool> IsTestExistAsync(Guid id)
    {
        var test = await _context.Tests
            .Where(x => x.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return test != null;
    }

    public async Task<bool> CreateTestAsync(Test test)
    {
        if (test != null)
        {
            var oldTest = await GetTestByIdAsync(test.Id);

            if (oldTest != null)
            {
                return false;
            }

            _context.Tests.Add(test);
            await _context.SaveChangesAsync();

            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> UpdateTestAsync(Test test)
    {
        if (test == null)
        {
            return false;
        }

        _context.Update(test);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteTestAsync(Test test)
    {
        if (test != null)
        {
            _context.Remove(test!);
            await _context.SaveChangesAsync();

            return true;
        }
        else
        {
            return false;
        }
    }
}
