using System.Net.Mime;
using BookService.Database;
using BookService.Database.Models;
using BookService.Utils;
using Microsoft.EntityFrameworkCore;

namespace BookService;

public static class Books
{
    public static List<Book> GetAvailableBooks()
    {
        using var context = new BooksContext();

        var books = context.Books.Where(b => b.Count > 0).ToList();
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

        var imageName = Images.SaveImage(book.Image);
        if (imageName == string.Empty)
        {
            return;
        }

        context.Entry(book).State = EntityState.Detached;

        var newBook = new Book
        {
            Image = imageName,
            AuthorId = book.AuthorId,
            GenreId = book.GenreId,
            Cost = book.Cost,
            Year = book.Year,
            Name = book.Name,
            Count = book.Count,
            Description = book.Description,
        };

        context.Books.Add(newBook);

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

        var oldBook = await context.Books.FirstOrDefaultAsync(b => b.Id == book.Id);

        if (oldBook is null)
        {
            return;
        }

        var file = Path.GetFileName(book.Image);
        if (oldBook.Image != file)
        {
            Images.RemoveImage(oldBook.Image);
            var fileName = Images.SaveImage(book.Image);
            if (fileName != string.Empty)
            {
                oldBook.Image = fileName;
            }
        }

        context.Entry(book).State = EntityState.Detached;

        oldBook.Image = book.Image;
        oldBook.AuthorId = book.AuthorId;
        oldBook.GenreId = book.GenreId;
        oldBook.Cost = book.Cost;
        oldBook.Year = book.Year;
        oldBook.Name = book.Name;
        oldBook.Count = book.Count;
        oldBook.Description = book.Description;

        oldBook.Image = Path.GetFileName(oldBook.Image);

        context.Books.Update(oldBook);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public static async Task RemoveRange(List<Book> books)
    {
        await using var context = new BooksContext();

        books.ForEach(b => { Images.RemoveImage(Path.GetFileName(b.Image)); });

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