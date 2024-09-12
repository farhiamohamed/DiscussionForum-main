using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace DiscussionForum.Models
{
    public class User : IdentityUser
    {
        public DateTime Created { get; set; } = DateTime.Now;
        public string? ImageUrl { get; set; }
    }
}