using System;
using System.ComponentModel.DataAnnotations;
using DiscussionForum.Models;

namespace DiscussionForum.ViewModels;

public class QuestionDetailViewModel
{

    [StringLength(1000, ErrorMessage = "Reply exceeds the maximum allowed length of 1000 characters")]
    [Display(Name = "Reply")]
    public string ReplyContent { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;
	public Question Question { get; set; } = default!;

    public QuestionDetailViewModel()
    {
        
    }

    public QuestionDetailViewModel(Question question, string userId)
	{
		Question = question;
        UserId = userId;
	}
}

