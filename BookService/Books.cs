using BookService.Database;
using BookService.Database.Models;
using BookService.Utils;
using Microsoft.EntityFrameworkCore;

namespace BookService;

public static class Books
{
    public static List<Book> GetAll()
    {
        using var context = new BooksContext();

        var books = context.Books.ToList();
        books.ForEach(b => b.Image = Images.FullName(b.Image));

        return books;
    }
    
    public static List<Book> GetAllWithRelative()
    {
        using var context = new BooksContext();

        var books = context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .ToList();
        books.ForEach(b => b.Image = Images.FullName(b.Image));

        return books;
    }

    public static Book? GetById(long id)
    {
        using var context = new BooksContext();

        var book = context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .Include(b => b.Status)
            .FirstOrDefault(b => b.Id == id);

        if (book is not null)
        {
            book.Image = Images.FullName(book.Image);
        }

        return book;
    }

    public static async Task SaveBook(Book book)
    {
        await using var context = new BooksContext();

        context.Books.Add(book);
        
        try
        {
            await context.SaveChangesAsync();
        }
        catch
        {
            //ignored
        }
    }

    public static async Task UpdateBook(Book book)
    {
        await using var context = new BooksContext();

        context.Books.Update(book);
        
        try
        {
            await context.SaveChangesAsync();
        }
        catch
        {
            //ignored
        }
    }

    public static async Task RemoveRange(List<Book> books)
    {
        await using var context = new BooksContext();
        
        context.RemoveRange(books);

        try
        {
            await context.SaveChangesAsync();
        }
        catch
        {
            //ignored
        }
    }

    public static List<Author> GetAuthors()
    {
        using var context = new BooksContext();

        return context.Authors.ToList();
    }

    public static List<Genre> GetGenres()
    {
        using var context = new BooksContext();

        return context.Genres.ToList();
    }
}