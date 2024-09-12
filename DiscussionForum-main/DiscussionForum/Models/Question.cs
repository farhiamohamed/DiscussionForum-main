using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Hosting;

namespace DiscussionForum.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }

        [StringLength(200, ErrorMessage = "Title exceeds the maximum allowed length of 200 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Question exceeds the maximum allowed length of 1000 characters")]
        [Display(Name = "Question")]
        public string Content { get; set; } = string.Empty;

        public DateTime Created { get; set; } = DateTime.Now;

        [ForeignKey("User")]
        public string Id { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        
        public virtual List<Reply>? Replies { get; set; }
        public virtual User User { get; set; } = default!;
        public virtual Category Category { get; set; } = default!;
    }
}

