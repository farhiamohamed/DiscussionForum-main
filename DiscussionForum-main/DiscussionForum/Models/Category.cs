using System;
using System.ComponentModel.DataAnnotations;

namespace DiscussionForum.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [StringLength(100, ErrorMessage = "Title exceeds the maximum allowed length of 100 characters")]
        public string Title { get; set; } = string.Empty;
    }
}

