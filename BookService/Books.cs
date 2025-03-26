using BookService.Database;
using BookService.Database.models;
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
}