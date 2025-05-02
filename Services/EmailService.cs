using BookHive.DTOs;
using BookHive.Interfaces;
using System.Net.Mail;

namespace BookHive.Services;
public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(EmailDto email)
    {
        using var client = new SmtpClient
        {
            Host = _configuration["Smtp:Host"],
            Port = int.Parse(_configuration["Smtp:Port"]),
            EnableSsl = bool.Parse(_configuration["Smtp:EnableSsl"]),
            Credentials = new System.Net.NetworkCredential(
                _configuration["Smtp:Username"],
                _configuration["Smtp:Password"])
        };

        var message = new MailMessage
        {
            From = new MailAddress(_configuration["Smtp:FromEmail"], "BookHive"),
            Subject = email.Subject,
            Body = email.Body,
            IsBodyHtml = true
        };
        message.To.Add(email.To);

        await client.SendMailAsync(message);
    }
}