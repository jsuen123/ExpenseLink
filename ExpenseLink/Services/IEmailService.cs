using System.Net.Mail;

namespace ExpenseLink.Services
{
    public interface IEmailService
    {
        void Send(MailMessage mailMessage);
    }
}