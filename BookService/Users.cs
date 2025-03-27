using System.Text;
using BookService.Database;
using BookService.Database.models;
using Microsoft.EntityFrameworkCore;

namespace BookService;

public static class Users
{
    private const string UserAlreadyExistsError = "Пользователь с таким email уже существует";
    private const string UserNotFoundError = "Пользователь с таким email не найден";

    public static async Task<User?> Authorize(string? email, string? password)
    {
        await using var context = new BooksContext();

        var user = await context.Users.Include(user => user.Role).FirstOrDefaultAsync(user => user.Email == email);

        if (user == null)
        {
            return null;
        }

        return !BCrypt.Net.BCrypt.Verify(password, Encoding.UTF8.GetString(user.Password)) ? null : user;
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

    public static async Task<bool> IsEmailExists(string? email)
    {
        await using var context = new BooksContext();

        return await context.Users.AnyAsync(u => u.Email == email);
    }

    public static async Task<string> ChangePassword(string? email, string? password)
    {
        await using var context = new BooksContext();
        
        var user = await context.Users.FirstOrDefaultAsync(user => user.Email == email);

        if (user is null)
        {
            return UserNotFoundError;
        }
        
        var passHash = Encoding.UTF8.GetBytes(
            BCrypt.Net.BCrypt.HashPassword(
                password,
                BCrypt.Net.BCrypt.GenerateSalt(10)
            )
        );

        user.Password = passHash;

        context.Users.Update(user);
        
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