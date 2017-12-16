using Infrastructure.ContactProvider;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SpikeConnectProviders.Extensions
{
    public static class EmailSenderExtension
    {
        public static Task SendConfirmationEmail(this IEmailSender emailSender, string email, string link)
        {
            string subject = "Confirm your email";
            string message = $"Click this <a href='{HtmlEncoder.Default.Encode(link)}'>link</a> to confirm your email";
            return emailSender.SendEmailAsync(email, subject, message);
        }
    }
}
