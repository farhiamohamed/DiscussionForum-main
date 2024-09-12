using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DiscussionForum.DAL;
using DiscussionForum.Models;
using DiscussionForum.ViewModels;
using DiscussionForum.Utilities;
using System.IO.Pipelines;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections;


namespace DiscussionForum.Controllers;

public class QuestionController : Controller
{
    private readonly IQuestionRepository _questionRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<QuestionController> _logger;

    //Stores the number of questions displayed per page
    private int pageSize = 3;

    public QuestionController(IQuestionRepository questionRepository, ICategoryRepository categoryRepository,
        ILogger<QuestionController> logger)
    {
        _questionRepository = questionRepository;
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    //Retrieves a question based on it's ID
    public async Task<IActionResult> Details(int id)
    {
        var question = await _questionRepository.GetQuestionById(id);
        if (question == null)
        {
            _logger.LogError("[QuestionController] Question not found for the QuestionId {QuestionId: 0000}", id);
            return NotFound("Question not found for the QuestionId");
        }

        //Stores the currently authenticated user's ID
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var QuestionDetailViewModel = new QuestionDetailViewModel(question, userId);

        return View(QuestionDetailViewModel);
    }

    //Retrieves a list of questions based on an optional search query and a page number
    public async Task<IActionResult> AllQuestions(string? searchQuery, int? pageNr)
    {
        IEnumerable<Question>? questions;
        int count;

        //Checks if a search query is provided and, if so, retrieves search results from the repository
        if (!string.IsNullOrEmpty(searchQuery))
        {
            var searchResults = await _questionRepository.GetSearchResultsPaged(searchQuery, pageNr, pageSize);
            questions = searchResults.Results;

            if (questions == null)
            {
                _logger.LogError("[QuestionController] Question list not found while executing _questionRepository.GetSearchResultsPaged()");
                return NotFound("Question list not found");
            }
            
            count = searchResults.Count;
        }

        //Retrieves all questions from the repository
        else
        {
            questions = await _questionRepository.GetAllQuestionsPaged(pageNr, pageSize);

            if (questions == null)
            {
                _logger.LogError("[QuestionController] Question list not found while executing _questionRepository.GetAllQuestionsPaged()");
                return NotFound("Question list not found");
            }

            count = await _questionRepository.GetQuestionsCount();
        }

        //Ensures that the pageNr and searchQuery have default values if they are null
        pageNr ??= 1;
        searchQuery ??= "";

        //Used for managing paginated questions
        var paginatedList = new PaginatedList<Question>(questions.ToList(), count, pageNr.Value, pageSize);


        var QuestionListViewModel = new QuestionListViewModel(paginatedList, searchQuery);

        return View(QuestionListViewModel);
    }

    //Retrieves a list of questions associated with the currently logged-in user
    [Authorize]
    public async Task<IActionResult> UserQuestions()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var questions = await _questionRepository.GetQuestionsByUserId(userId);

        if(questions == null)
        {
            _logger.LogError("[QuestionController] Question list for user not found while executing _questionRepository.GetQuestionsByUserId(userId)");
            return NotFound("Question list not found");
        }

        return View(questions);
    }

    //Responsible for the deletion of a question by its ID
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
       bool returnOK = await _questionRepository.Delete(id);
        if(!returnOK)
        {
            _logger.LogError("[QuestionController] Question deletion failed for the QuestionId {QuestionId: 0000}", id);
            return BadRequest("Question deletion failed");
        }

        return RedirectToAction(nameof(UserQuestions));
    }

    //Fetches a question based on it's ID, and the list of categories for updating purpose
    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var question = await _questionRepository.GetQuestionById(id);
        var CategorySelectList = await _categoryRepository.GetCategorySelectList();

        if (question == null)
        {
            _logger.LogError("[QuestionController] Question not found when retrieving the QuestionId {QuestionId: 0000} from update", id);
            return BadRequest("Question not found for the QuestionId");
        }

        if (CategorySelectList == null)
        {
            _logger.LogError("[QuestionController] Category list not found when retrieving the QuestionId {QuestionId: 0000} from update", id);
            return BadRequest("Category list not found");
        }

        var UpdateQuestionViewModel = new QuestionViewModel(question, CategorySelectList);
        return View(UpdateQuestionViewModel);
    }

    //Validates the provided question data, and if it's valid, it updates the question
    //If validation fails, the same view is returned displaying all available categories
    [HttpPost]
    public async Task<IActionResult> Update(Question question)
    {
        if (question.QuestionId > 0 && question.CategoryId > 0 && !string.IsNullOrEmpty(question.Id) && !string.IsNullOrEmpty(question.Title) && !string.IsNullOrEmpty(question.Content))
        {
            question.Created = DateTime.Now;
            bool updateOk = await _questionRepository.Update(question);
            if(updateOk)
                return RedirectToAction(nameof(UserQuestions));
        }

        _logger.LogWarning("[QuestionController] Question update failed {@question}", question);

        var CategorySelectList = await _categoryRepository.GetCategorySelectList();

        if (CategorySelectList == null)
        {
            _logger.LogError("[QuestionController] Category list not found when updating the QuestionId {QuestionId: 0000}", question.QuestionId);
            return BadRequest("Category list not found");
        }

        var UpdateQuestionViewModel = new QuestionViewModel(question, CategorySelectList);
        return View(UpdateQuestionViewModel);
    }

    //Fetches the list of categories and displays it for question creation purpose
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Create()
    {
        var CategorySelectList = await _categoryRepository.GetCategorySelectList();

        if (CategorySelectList == null)
        {
            _logger.LogError("[QuestionController] Category list not found when retrieving from Create");
            return BadRequest("Category list not found");
        }

        var CreateQuestionViewModel = new QuestionViewModel(CategorySelectList);
        return View(CreateQuestionViewModel);
    }

    //Validates the provided question data, and if it's valid, it creates a new question
    //If validation fails, the same view is returned displaying all available categories
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(Question question)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (question.CategoryId > 0 && !string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(question.Title) && !string.IsNullOrEmpty(question.Content))
        {
            question.Id = userId;
            question.Created = DateTime.Now;

            bool createOk = await _questionRepository.Create(question);
            if (createOk)
                return RedirectToAction(nameof(AllQuestions));
        }
        _logger.LogWarning("[QuestionController] Question creation failed {@question}", question);

        var CategorySelectList = await _categoryRepository.GetCategorySelectList();

        if (CategorySelectList == null)
        {
            _logger.LogError("[QuestionController] Category list not found when creating question");
            return BadRequest("Category list not found");
        }

        var CreateQuestionViewModel = new QuestionViewModel(CategorySelectList);
        return View(CreateQuestionViewModel);
    }

}