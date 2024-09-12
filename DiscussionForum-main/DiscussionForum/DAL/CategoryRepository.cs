using System;
using DiscussionForum.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.DAL;

public class CategoryRepository : ICategoryRepository
{
    private readonly ForumDbContext _db;
    private readonly ILogger<QuestionRepository> _logger;

    public CategoryRepository(ForumDbContext db, ILogger<QuestionRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    //Used to retrieve all categories from the database
    public async Task<IEnumerable<Category>?> GetAll()
    {
        try
        {
            return await _db.Categories.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[CategoryRepository] categories ToListAsync() failed when GetAll(), error" +
                "message: {e}", e.Message);
            return null;
        }
    }

    //Used to create a list of SelectListItem objects based on the categories retrieved using the GetAll() method
    public async Task<List<SelectListItem>?> GetCategorySelectList()
    {
        var categories = await GetAll();
        if (categories == null)
            return null;

        try
        {
            return categories.Select(category => new SelectListItem
            {
                Value = category.CategoryId.ToString(),
                Text = category.Title
            }).ToList();
        }
        catch(Exception e)
        {
            _logger.LogError("[CategoryRepository] categories ToList() failed when creating select list, error" +
                "message: {e}", e.Message);
            return null;
        }
    }
}