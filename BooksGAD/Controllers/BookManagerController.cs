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

    [HttpGet("{bookName}")]
    public async Task<IActionResult> GetById(string bookName)
    {
        var book = await _bookManagerRepository.GetBook(bookName);
        if (book == null)
            return NotFound(new { Error = $"Book with name {bookName} not found" });
        return Ok(book);
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
    public async Task<IActionResult> Delete(string bookName)
    {
        var existingBook = await _bookManagerRepository.GetBook(bookName);
        if (existingBook == null)
            return BadRequest(new { Error = $"Book with title {bookName} not found" });
        await _bookManagerRepository.DeleteBook(bookName);
        return Ok(new { Message = $"Book deleted successfully" });
    }

    [HttpPost("FullText/{searchString}")]
    public async Task<IActionResult> SearchByFullText(string searchString)
    {
        if (!string.IsNullOrEmpty(searchString))
        {
            var bookList = await _bookManagerRepository.GetBooksByFullText(searchString);
            return Ok(bookList);
        }
        return BadRequest(new { Error = "Search string in empty" });
    }

    [HttpPost("Regex")]
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