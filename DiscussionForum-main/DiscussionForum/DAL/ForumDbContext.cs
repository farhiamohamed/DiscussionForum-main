using System;
using DiscussionForum.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace DiscussionForum.DAL;

public class ForumDbContext : IdentityDbContext<User>
{
    public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options)
    {
        //Database.EnsureCreated();
    }

    public DbSet<User> ForumUsers { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Reply> Replies { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }
}