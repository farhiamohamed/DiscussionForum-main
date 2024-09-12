using System;
using DiscussionForum.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiscussionForum.ViewModels
{
    public class QuestionViewModel
    {
        public Question Question { get; set; } = default!;
        public List<SelectListItem> CategorySelectList { get; set; } = default!;

        public QuestionViewModel(List<SelectListItem> categorySelectList)
        {
            CategorySelectList = categorySelectList;
        }

        public QuestionViewModel(Question question, List<SelectListItem> categorySelectList)
        {
            Question = question;
            CategorySelectList = categorySelectList;
        }
    }
}
