using BooksGAD.Models;
using BooksGAD.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BooksGAD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookManagerController : ControllerBase
{
    private readonly ILogger<BookManagerController> _logger;
    private readonly IBookManagerRepository _bookManagerRepository;
    
    public BookManagerController(ILogger<BookManagerController> logger, IBookManagerRepository bookManagerRepository)
    {
        _logger = logger;
        _bookManagerRepository = bookManagerRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var bookList = await _bookManagerRepository.GetBooks();
        return Ok(bookList);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BookCreatingFormDto dto)
    {
        var existingBook = await _bookManagerRepository.GetBook(dto.Name);
        if (existingBook != null)
            return BadRequest(new { Error = $"book with title {dto.Name} is exists " });

        var newBook = await _bookManagerRepository.AddBook(dto.Name, dto.Authors, dto.Annotation);
        return Ok(newBook);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] string title)
    {
        var existingBook = await _bookManagerRepository.GetBook(title);
        if (existingBook == null)
            return BadRequest(new { Error = $"Book with title {title} not found" });
        await _bookManagerRepository.DeleteBook(title);
        return Ok(new { Message = $"Book deleted successfully" });
    }

    [HttpGet("FullText")]
    public async Task<IActionResult> SearchByFullText([FromQuery] string searchQuery)
    {
        if (!string.IsNullOrEmpty(searchQuery))
        {
            var bookList = await _bookManagerRepository.GetBooksByFullText(searchQuery);
            return Ok(bookList);
        }
        return BadRequest(new { Error = "Search string in empty" });
    }

    [HttpGet("Regex")]
    public async Task<IActionResult> SearchByRegex([FromQuery] string regexString)
    {
        if (!string.IsNullOrEmpty(regexString))
        {
            var bookList = await _bookManagerRepository.GetBooksByRegex(regexString);
            return Ok(bookList);
        }
        return BadRequest(new { Error = "Regex string in empty" });
    }
}