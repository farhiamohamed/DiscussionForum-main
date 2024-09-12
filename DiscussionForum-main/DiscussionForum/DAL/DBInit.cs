using Microsoft.EntityFrameworkCore;
using DiscussionForum.DAL;
using Microsoft.Extensions.Hosting;

namespace DiscussionForum.Models;

public static class DBInit
{
    //ApplicationsBuilder contains access to the services
    public static void Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        ForumDbContext context = serviceScope.ServiceProvider.GetRequiredService<ForumDbContext>();
        //context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        if (!context.Categories.Any())
        {
            var categories = new List<Category>
            {
                new Category {Title = "Programming"},
                new Category {Title = "Web/app development"},
                new Category {Title = "Network & Security"},
                new Category {Title = "Software apps"},
                new Category {Title = "Hardware"},
                new Category {Title = "OS"},
                new Category {Title = "Databases"},
                new Category {Title = "AI & Data Science"},
                new Category {Title = "General IT"},
            };
            context.AddRange(categories);
            context.SaveChanges();
        }

        context.SaveChanges();
    }
}