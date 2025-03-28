using System;
using System.Net.Mail;

namespace App.Domain.Utils;

public static class Utils
{
    public static bool IsValidEmail(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith('.'))
        {
            return false;
        }

        try
        {
            var addr = new MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }

    public static string GenerateRandomDigits(int length)
    {
        var random = new Random();
        var result = string.Empty;

        for (int i = 0; i < length; i++)
        {
            result += random.Next(0, 10).ToString();
        }

        return result;
    }
}