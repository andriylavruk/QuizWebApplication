using Microsoft.EntityFrameworkCore;
using QuizApp.Server.Data;
using QuizApp.Server.Repositories.Interfaces;
using QuizApp.Shared.DTO;
using QuizApp.Shared.Models;

namespace QuizApp.Server.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly DataContext _context;

    public QuestionRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Question>> GetAllQuestionsAsync()
    {
        return await _context.Questions
            .Include(x => x.Test)
            .ToListAsync();
    }

    public async Task<List<QuestionForTestParticipantDTO>> GetQuestionsForTestParticipantAsync(Guid id)
    {
        var participant = await _context.TestParticipants
            .FirstOrDefaultAsync(x => x.UserId == id);

        if (participant == null)
        {
            return new List<QuestionForTestParticipantDTO>();
        }

        var questions =
            from ques in _context.Questions
            where ques.TestId == participant!.TestId
            select new QuestionForTestParticipantDTO 
            { 
                Id = ques.Id,
                Description = ques.Description,
                TestId = ques.TestId,
                Test = ques.Test,
                Options = new string[]
                {
                    ques.Option1,
                    ques.Option2,
                    ques.Option3,
                    ques.Option4
                }
            };

        return await questions.OrderBy(x => Guid.NewGuid()).ToListAsync();
    }

    public async Task<List<Question>> GetQuestionsByTestIdAsync(Guid testId)
    {
        return await _context.Questions
            .Include(x => x.Test)
            .Where(x => x.Test!.Id == testId).ToListAsync();
    }

    public async Task<Question?> GetQuestionByIdAsync(Guid id)
    {
        return await _context.Questions.SingleOrDefaultAsync(r => r.Id == id);
    }

    public async Task<bool> IsQuestionExistAsync(Guid id)
    {
        var question = await _context.Questions
            .Where(x => x.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return question != null;
    }

    public async Task<bool> CreateQuestionAsync(Question question)
    {
        if (question != null)
        {
            var oldQuestion = await GetQuestionByIdAsync(question.Id);

            if (oldQuestion != null)
            {
                return false;
            }

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> UpdateQuestionAsync(Question question)
    {
        if (question == null)
        {
            return false;
        }

        _context.Update(question);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteQuestionAsync(Question question)
    {
        if (question != null)
        {
            _context.Remove(question!);
            await _context.SaveChangesAsync();

            return true;
        }
        else
        {
            return false;
        }
    }
}
