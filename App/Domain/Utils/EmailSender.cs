namespace App.Domain.Utils;

using System;
using MailKit.Net.Smtp;
using MimeKit;
using DotNetEnv;

public static class EmailSender
{
    private static readonly Lazy<EmailSettings> _settings;

    static EmailSender()
    {
        _settings = new Lazy<EmailSettings>(() =>
        {
            Env.Load();
            
            return new EmailSettings
            {
                Host = Env.GetString("SMTP_HOST"),
                Port = Env.GetInt("SMTP_PORT"),
                User = Env.GetString("SMTP_USER"),
                Password = Env.GetString("SMTP_PASSWORD"),
                UseSsl = Env.GetBool("SMTP_USE_SSL"),
                FromName = Env.GetString("FROM_NAME"),
                FromEmail = Env.GetString("FROM_EMAIL")
            };
        });
    }

    public static void Send(string toEmail, string toName, string subject, string text, bool isHtml = false)
    {
        try
        {
            var settings = _settings.Value;
            var message = new MimeMessage();
            
            message.From.Add(new MailboxAddress(settings.FromName, settings.FromEmail));
            message.To.Add(new MailboxAddress(toName, toEmail));
            message.Subject = subject;

            message.Body = isHtml 
                ? new TextPart("html") { Text = text } 
                : new TextPart("plain") { Text = text };

            using (var client = new SmtpClient())
            {
                client.Connect(settings.Host, settings.Port, settings.UseSsl);
                client.Authenticate(settings.User, settings.Password);
                client.Send(message);
                client.Disconnect(true);
            }

            Console.WriteLine("Письмо отправлено!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    private class EmailSettings
    {
        public string Host { get; init; }
        public int Port { get; init; }
        public string User { get; init; }
        public string Password { get; init; }
        public bool UseSsl { get; init; }
        public string FromName { get; init; }
        public string FromEmail { get; init; }
    }
}