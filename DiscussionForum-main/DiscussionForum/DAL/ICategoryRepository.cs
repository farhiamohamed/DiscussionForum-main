using System;
using DiscussionForum.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiscussionForum.DAL;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>?> GetAll();
    Task<List<SelectListItem>?> GetCategorySelectList();
}
