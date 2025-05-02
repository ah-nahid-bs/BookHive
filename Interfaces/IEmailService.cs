using BookHive.DTOs;

namespace BookHive.Interfaces;
public interface IEmailService
{
    Task SendEmailAsync(EmailDto emailDto);
}