using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DiscussionForum.DAL;
using DiscussionForum.Models;
using DiscussionForum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DiscussionForum.Controllers;

public class ReplyController : Controller
{
    private readonly IReplyRepository _replyRepository;
    private readonly ILogger<ReplyController> _logger;

    public ReplyController(IReplyRepository replyRepository, ILogger<ReplyController> logger)
    {
        _replyRepository = replyRepository;
        _logger = logger;
    }

    //Handles the creation of a reply to a question
    //Validates user input and redirects to different actions based on the result
    [Authorize]
    public async Task<IActionResult> Create(QuestionDetailViewModel questionDetailViewModel)
    {

        //Stores the currently authenticated user's ID
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var questionId = questionDetailViewModel.Question?.QuestionId;
        var content = questionDetailViewModel.ReplyContent;

        if (questionId > 0 && !string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(content))
        {
            var newReply = new Reply
            {
                QuestionId = questionId.Value,
                Id = userId,
                Content = content,
                Created = DateTime.Now
            };

            bool createdOk = await _replyRepository.Create(newReply);
            //Redirects to the "AllQuestions" action if the creation was successful
            if (createdOk)
                return RedirectToAction("AllQuestions", "Question");
        }

        _logger.LogWarning("[ReplyController] Reply creation could not be done");

        //Redirects to the "Details" action
        if (questionId.HasValue)
        {
            return RedirectToAction("Details", "Question", new { id = questionId.Value });
        }

        else
        {
            //In case the questionId is not available, attempt to redirect using the referrer URL
            var referrerUrl = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referrerUrl) && Url.IsLocalUrl(referrerUrl))
            {
                return Redirect(referrerUrl);
            }

            //If all else fails, redirect to the generic questions page
            return RedirectToAction("AllQuestions", "Question");
        }
    }

    //Handles the updating of a reply
    public async Task<IActionResult> Update(Reply reply)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (reply.ReplyId > 0 && reply.QuestionId > 0 && !string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(reply.Content))
        {
            reply.Id = userId;
            reply.Created = DateTime.Now;
            bool updateOk = await _replyRepository.Update(reply);
            if(updateOk)
                return RedirectToAction("AllQuestions", "Question");
        }
        _logger.LogWarning("[ReplyController] Reply update failed {@reply}", reply);

        return RedirectToAction("Details", "Question", new { id = reply.QuestionId });
    }

}