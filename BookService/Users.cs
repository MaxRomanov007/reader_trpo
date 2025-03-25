using System.Text;
using BookService.Database;
using BookService.Database.models;
using Microsoft.EntityFrameworkCore;

namespace BookService;

public static class Users
{
    private const string InvalidCredentialsError = "Неверные логин или пароль";
    private const string UserAlreadyExistsError = "Пользователь с таким email уже существует";

    public static async Task<string> Authorize(string? email, string? password)
    {
        await using var context = new BooksContext();

        var user = await context.Users.Include(user => user.Role).FirstOrDefaultAsync(user => user.Email == email);

        if (user == null)
        {
            return InvalidCredentialsError;
        }

        if (!BCrypt.Net.BCrypt.Verify(password, Encoding.UTF8.GetString(user.Password)))
        {
            return InvalidCredentialsError;
        }

        return user.Role.Name;
    }

    public static async Task<string> Register(string? email, string? password)
    {
        await using var context = new BooksContext();

        if (context.Users.Any(u => u.Email == email))
        {
            return UserAlreadyExistsError;
        }

        var passHash = Encoding.UTF8.GetBytes(
            BCrypt.Net.BCrypt.HashPassword(
                password,
                BCrypt.Net.BCrypt.GenerateSalt(10)
            )
        );

        context.Users.Add(new User
        {
            Password = passHash,
            Email = email ?? string.Empty,
        });

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return e.Message;
        }

        return string.Empty;
    }
}