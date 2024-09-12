using System;
using Microsoft.EntityFrameworkCore;
using DiscussionForum.Models;

namespace DiscussionForum.DAL;

public class ReplyRepository : IReplyRepository
{
    private readonly ForumDbContext _db;
    private readonly ILogger<ReplyRepository> _logger;

    public ReplyRepository(ForumDbContext db, ILogger<ReplyRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    //Creates a reply based on the provided reply data and returns a boolean indicating the success of the creation
    public async Task<bool> Create(Reply reply)
    {
        try
        {
            _db.Replies.Add(reply);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ReplyRepository] reply creation failed for reply {@reply}, error message:" +
                "{e}", reply, e.Message);
            return false;
        }
       
    }

    //Updates a reply based on the provided reply data and returns a boolean indicating the success of the update
    public async Task<bool> Update(Reply reply)
    {
        try
        {
            _db.Replies.Update(reply);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ReplyRepository] reply SaveChangesAsync() failed when updating the ReplyId" +
                "{ReplyId: 0000}, error message {e}", reply, e.Message);
            return false;
        }

    }
}

