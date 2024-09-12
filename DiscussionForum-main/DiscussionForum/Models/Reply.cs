using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Hosting;

namespace DiscussionForum.Models
{
    public class Reply
    {
        [Key]
        public int ReplyId { get; set; }

        [StringLength(1000, ErrorMessage = "Reply exceeds the maximum allowed length of 1000 characters")]
        [Display(Name = "Reply")]
        public string Content { get; set; } = string.Empty;

        public DateTime Created { get; set; } = DateTime.Now;

        public int QuestionId { get; set; }

        [ForeignKey("User")]
        public string Id { get; set; } = string.Empty;

        public virtual User User { get; set; } = default!;
    }

}