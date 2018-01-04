using System.Threading.Tasks;

namespace Spike.Service.Interface.ContactProvider
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string receiverEmail, string subject, string message);
        Task Execute(string apiKey, string receiverEmail, string subject, string message);
        Task SendConfirmationEmail(string email, string link);
        Task SendResetPasswordEmail(string email, string link);
    }
}
