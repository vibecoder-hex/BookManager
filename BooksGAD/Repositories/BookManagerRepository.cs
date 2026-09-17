using System.Text.RegularExpressions;
using BooksGAD.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksGAD.Repositories;

public interface IBookManagerRepository
{
    Task<List<BookFromDbDto>> GetBooks();
    Task<BookFromDbDto?> GetBook(string bookName);
    Task<Book?> AddBook(string name, string authors, string annotation);
    Task DeleteBook(string bookName);
    Task<List<BookFromDbDto>> GetBooksByFullText(string searchString);
    Task<List<BookFromDbDto>> GetBooksByRegex(string regexPattern);
}

public class BookManagerRepository : IBookManagerRepository
{
    private readonly BookManagerContext _dbContext;

    public BookManagerRepository(BookManagerContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<BookFromDbDto>> GetBooks()
    {
        var bookList = await _dbContext.Books
            .Select(b => new BookFromDbDto(b.Name, b.Authors, b.Annotation, b.PublishingDate))
            .ToListAsync();
        return bookList;
    }

    public async Task<BookFromDbDto?> GetBook(string bookName)
    {
        var book = await _dbContext.Books
            .Where(b => b.Name == bookName)
            .Select(b => new BookFromDbDto(b.Name, b.Authors, b.Annotation, b.PublishingDate))
            .FirstOrDefaultAsync();
        return book;
    }

    public async Task<Book?> AddBook(string name, string authors, string annotation)
    {
        var newBook = new Book
        {
            Name = name,
            Authors = authors,
            Annotation = annotation,
            PublishingDate = DateOnly.FromDateTime(DateTime.UtcNow)
        };
        _dbContext.Books.Add(newBook);
        await _dbContext.SaveChangesAsync();
        return newBook;
    }

    public async Task DeleteBook(string bookName)
    {
        var book = await _dbContext.Books
            .Where(b => b.Name == bookName)
            .FirstOrDefaultAsync();
        _dbContext.Books.Remove(book);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<BookFromDbDto>> GetBooksByFullText(string searchString)
    {
        var books = await _dbContext.Books
            .AsNoTracking()
            .Where(b => b.FtsAnnotation.Matches(EF.Functions.WebSearchToTsQuery("russian", searchString)))
            .Select(b =>  new BookFromDbDto(b.Name, b.Authors, b.Annotation, b.PublishingDate))
            .ToListAsync();
        
        return books;
    }

    public async Task<List<BookFromDbDto>> GetBooksByRegex(string regexPattern)
    {
        var bookList = await _dbContext.Books
            .AsNoTracking()
            .ToListAsync();
        
        var regexFilteredBooks = bookList
            .Where(book => Regex.IsMatch(book.Name, regexPattern) ||
                           Regex.IsMatch(book.Authors, regexPattern) ||
                           Regex.IsMatch(book.Annotation, regexPattern))
            .Select(book => new BookFromDbDto(book.Name, book.Authors, book.Annotation, book.PublishingDate))
            .ToList();
        return regexFilteredBooks;
    }
}