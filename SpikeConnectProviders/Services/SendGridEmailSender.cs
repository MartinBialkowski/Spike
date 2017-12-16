﻿using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using EFCoreSpike5.CommonModels;
using Microsoft.Extensions.Options;
using Infrastructure.ContactProvider;
using System.Text.Encodings.Web;

namespace SpikeConnectProviders
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
            string subject = "Confirm your email";
            string message = $"Click this <a href='{HtmlEncoder.Default.Encode(link)}'>link</a> to confirm your email";
            return SendEmailAsync(email, subject, message);
        }

        public Task SendEmailAsync(string receiverEmail, string subject, string message)
        {
            return Execute(Options.ApiKey, receiverEmail, subject, message);
        }

        public Task Execute(string apiKey, string receiverEmail, string subject, string message)
        {
            var client = new SendGridClient(apiKey);
            var sendGridMessage = new SendGridMessage()
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