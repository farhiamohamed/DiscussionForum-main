using System;
using DiscussionForum.Models;

namespace DiscussionForum.DAL;

public interface IReplyRepository
{
    Task<bool> Update(Reply reply); 
    Task<bool> Create(Reply reply);
}

