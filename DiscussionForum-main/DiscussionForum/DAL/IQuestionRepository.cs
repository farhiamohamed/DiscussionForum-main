using System;
using DiscussionForum.Models;

namespace DiscussionForum.DAL;

public interface IQuestionRepository
{
    Task<IEnumerable<Question>?> GetAll();
    Task<Question?> GetQuestionById(int id);
    Task<IEnumerable<Question>?> GetQuestionsByUserId(string id);
    Task<int> GetQuestionsCount();
    Task<IEnumerable<Question>?> GetAllQuestionsPaged(int? pageNr, int pageSize);
    Task<(IEnumerable<Question>? Results, int Count)> GetSearchResultsPaged(string searchQuery, int? pageNr, int pageSize);
    Task<bool> Create(Question question);
    Task <bool>Update(Question question);
    Task<bool> Delete(int id);
}