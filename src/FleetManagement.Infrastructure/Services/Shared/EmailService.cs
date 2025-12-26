using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using FleetManagement.Shared.Interfaces;

namespace FleetManagement.Infrastructure.Services.Shared
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> options, ILogger<EmailService> logger)
        {
            _settings = options.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            var emailMessage = new EmailMessage
            {
                To = to,
                Subject = subject,
                Body = body,
                IsHtml = isHtml
            };

            await SendEmailAsync(emailMessage);
        }

        public async Task SendEmailAsync(EmailMessage emailMessage)
        {
            try
            {
                using var client = new SmtpClient(_settings.Host, _settings.Port)
                {
                    Credentials = new NetworkCredential(_settings.Username, _settings.Password),
                    EnableSsl = _settings.EnableSsl
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_settings.From),
                    Subject = emailMessage.Subject,
                    Body = emailMessage.Body,
                    IsBodyHtml = emailMessage.IsHtml
                };
                mailMessage.To.Add(emailMessage.To);

                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Failed to send email to {Email}", emailMessage.To);

                // Optionally, you could retry, queue, or just swallow the exception
            }
        }
    }
}
