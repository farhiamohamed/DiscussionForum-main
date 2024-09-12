using System;
using System.Collections.Generic;
using DiscussionForum.Models;
using DiscussionForum.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiscussionForum.ViewModels;

public class QuestionListViewModel
{
    public PaginatedList<Question> PaginatedList { get; } = default!;
    public string? SearchQuery { get; set; } = string.Empty;

    public QuestionListViewModel(PaginatedList<Question> paginatedList, string? searchQuery)
    {
        PaginatedList = paginatedList;
        SearchQuery = searchQuery;
    }
}