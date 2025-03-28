using System;
using System.Threading.Tasks;
using DotNetEnv;
using MailKit.Net.Smtp;
using MimeKit;

namespace App.Domain.Utils;

public static class EmailSender
{
    private static readonly Lazy<EmailSettings> Settings;

    static EmailSender()
    {
        Settings = new Lazy<EmailSettings>(() =>
            new EmailSettings
            {
                Host = Env.GetString("SMTP_HOST"),
                Port = Env.GetInt("SMTP_PORT"),
                User = Env.GetString("SMTP_USER"),
                Password = Env.GetString("SMTP_PASSWORD"),
                UseSsl = Env.GetBool("SMTP_USE_SSL"),
                FromName = Env.GetString("FROM_NAME"),
                FromEmail = Env.GetString("FROM_EMAIL")
            }
        );
    }

    public static async Task SendAsync(string? toEmail, string subject, string text, bool isHtml = false)
    {
        var settings = Settings.Value;
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(settings.FromName, settings.FromEmail));
        message.To.Add(new MailboxAddress(string.Empty, toEmail));
        message.Subject = subject;

        message.Body = isHtml
            ? new TextPart("html") { Text = text }
            : new TextPart("plain") { Text = text };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(settings.Host, settings.Port, settings.UseSsl);
            await client.AuthenticateAsync(settings.User, settings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }

    private class EmailSettings
    {
        public string Host { get; init; } = string.Empty;
        public int Port { get; init; }
        public string User { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public bool UseSsl { get; init; }
        public string FromName { get; init; } = string.Empty;
        public string FromEmail { get; init; } = string.Empty;
    }
}