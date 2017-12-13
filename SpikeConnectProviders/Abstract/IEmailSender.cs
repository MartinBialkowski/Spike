using System.Threading.Tasks;

namespace SpikeConnectProviders.Abstract
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string receiverEmail, string subject, string message);

        Task Execute(string apiKey, string receiverEmail, string subject, string message);
    }
}