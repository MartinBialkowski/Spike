using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Spike.Backend.Connect.Model;
using Spike.Backend.Interface.Contact;

namespace Spike.Backend.Connect.Services
{
    public class SendGridEmailSender : IEmailSender
    {
        public SendGridEmailSender(IOptions<SendGridOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public SendGridOptions Options { get; set; }

        public Task SendConfirmationEmail(string email, string link)
        {
            const string subject = "Confirm your email";
            var message = $"Click this <a href='{HtmlEncoder.Default.Encode(link)}'>link</a> to confirm your email";
            return SendEmailAsync(email, subject, message);
        }

        public Task SendResetPasswordEmail(string email, string code)
        {
	        const string subject = "Reset password";
            var message = $"Use token below to reset password \n{code}";
            return SendEmailAsync(email, subject, message);
        }

        public Task SendEmailAsync(string receiverEmail, string subject, string message)
        {
            return Execute(Options.ApiKey, receiverEmail, subject, message);
        }

        public Task Execute(string apiKey, string receiverEmail, string subject, string message)
        {
            var client = new SendGridClient(apiKey);
            var sendGridMessage = new SendGridMessage
            {
                From = new EmailAddress(Options.SenderEmail, Options.SenderName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            sendGridMessage.AddTo(receiverEmail);
            return client.SendEmailAsync(sendGridMessage);
        }
    }
}