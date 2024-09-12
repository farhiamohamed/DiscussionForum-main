using System;
using Microsoft.EntityFrameworkCore;
using DiscussionForum.Models;

namespace DiscussionForum.DAL;

public class QuestionRepository : IQuestionRepository
{
    private readonly ForumDbContext _db;
    private readonly ILogger<QuestionRepository> _logger;

    public QuestionRepository(ForumDbContext db, ILogger<QuestionRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    //Used to retrieve a list of all the questions from the database
    public async Task<IEnumerable<Question>?> GetAll()
    {
        try
        {
            return await _db.Questions.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[QuestionRepository] questions ToListAsync() failed when GetAll(), error" +
                "message: {e}", e.Message);
            return null;
        }
    }

    //Used to retrieve a single question from the database by its ID
    public async Task<Question?> GetQuestionById(int id)
    {
        try
        {
            return await _db.Questions.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[QuestionRepository] question FindAsync(id) failed when GetQuestionById for " +
                "QuestionId {QuestionId: 0000}, error message: {e}", e.Message);
            return null;
        }
    }

    //Used to retrieve a list of questions based on the provided user ID
    public async Task<IEnumerable<Question>?> GetQuestionsByUserId(string id)
    {
        try
        {
            return await _db.Questions.Where(question => question.Id == id).ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[QuestionRepository] question ToListAsync() failed when GetQuestionsByUserId for " +
                "Id {Id: 0000}, error message: {e}", e.Message);
            return null;
        }
    }

    //Used to retrieve the total count of questions
    public async Task<int> GetQuestionsCount()
    {
        try
        {
            return await _db.Questions.CountAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[QuestionRepository] question CountAsync() failed when GetQuestionsCount, error message: {e}", e.Message);
            return 0;
        }
    }

    //Takes an IQueryable of questions and paginates them based on the provided page number and page size
    private async Task<IEnumerable<Question>?> PaginateQuestions(IQueryable<Question> questions, int? pageNr, int pageSize)
    {
        try
        {
            pageNr ??= 1;
            //Skips the questions from previous pages and takes only the questions for the current page based on the specified page size
            questions = questions.Skip((pageNr.Value - 1) * pageSize).Take(pageSize);

            return await questions.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[QuestionRepository] question ToListAsync() failed when PaginateQuestions, error message: {e}", e.Message);
            return null;
        }
    }

    //Returns a paginated list of all questions using the PaginateQuestions method 
    public async Task<IEnumerable<Question>?> GetAllQuestionsPaged(int? pageNr, int pageSize)
    {
        try
        {
            var questions = from question in _db.Questions select question;
            return await PaginateQuestions(questions, pageNr, pageSize);
        }
        catch (Exception e)
        {
            _logger.LogError("[QuestionRepository] question retrieval failed when GetAllQuestionsPaged, error message: {e}", e.Message);
            return null;
        }
    }

    //Filters and counts questions that match the search query and paginates the results using the PaginateQuestions method 
    //Returns both the paginated results and the total count of matching questions
    public async Task<(IEnumerable<Question>? Results, int Count)> GetSearchResultsPaged(string searchQuery, int? pageNr, int pageSize)
    {

        try
        {
            var questions = from question in _db.Questions select question;
            int count = 0;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                questions = questions.Where(question => question.Title.ToLower().Contains(searchQuery));
                count = await questions.CountAsync();
            }

            var pagedResults = await PaginateQuestions(questions, pageNr, pageSize);
            return (pagedResults, count);
        }
        catch(Exception e)
        {
            _logger.LogError("[QuestionRepository] paginating question search results failed when GetSearchResultsPaged, error message: {e}", e.Message);
            return (null, 0);
        }
    }

    //Deletes a question based on it's ID and returns a boolean indicating the success of the deletion
    public async Task<bool> Delete(int id)
    {
        try
        {
            var question = await _db.Questions.FindAsync(id);
            if (question == null)
            {
                _logger.LogError("[QuestionRepository] question not found for the QuestionId {QuestionId:0000}", id);
                return false;
            }

            _db.Questions.Remove(question);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[QuestionRepository] question deletion failed for the QuestionId {QuestionId: 0000}" +
                ", error message: {e}", id, e.Message);
            return false;
        }
    }

    //Updates a question based on the provided question data and returns a boolean indicating the success of the update
    public async Task<bool> Update(Question question)
    {
        try {
            _db.Questions.Update(question);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e) {
            _logger.LogError("[QuestionRepository] question SaveChangesAsync() failed when updating the QuestionId" +
                "{QuestionId: 0000}, error message {e}", question, e.Message);
            return false;
        }
    }

    //Creates a question based on the provided question data and returns a boolean indicating the success of the creation
    public async Task<bool> Create(Question question)
    {
        try
        {
            _db.Questions.Add(question);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[QuestionRepository] question creation failed for question {@question}, " +
                "error message: {e}", question, e.Message);
            return false;
        }
    }
}

