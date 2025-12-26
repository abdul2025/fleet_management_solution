using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FleetManagement.Shared.Interfaces;

namespace FleetManagement.Application.Shared.Services
{
    public class NotificationService
    {
        private readonly IEmailService _emailService;

        public NotificationService(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task SendWelcomeEmail(string userEmail, string userName)
        {
            var body = $"Hello {userName}, welcome to our platform!";
            await _emailService.SendEmailAsync(userEmail, "Welcome!", body);
        }
    }
}