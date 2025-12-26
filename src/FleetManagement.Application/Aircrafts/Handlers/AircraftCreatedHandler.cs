using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FleetManagement.Domain.Aircrafts.Events;
using FleetManagement.Shared.Interfaces;

namespace FleetManagement.Application.Aircrafts.Handlers
{
    
    public class AircraftCreatedHandler
    {
        private readonly IEmailService _emailService;

        // Inject IEmailService via constructor
        public AircraftCreatedHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Handle(AircraftCreatedEvent @event)
        {
            Console.WriteLine($"Aircraft {@event.RegistrationNumber} created at {@event.OccurredOn:O}");


            
            // Example: send notification email
            var subject = $"New Aircraft Created: {@event.RegistrationNumber}";
            var body = $"Aircraft {@event.RegistrationNumber} was created on {@event.OccurredOn:O}";

            await _emailService.SendEmailAsync("abdul.2020alsh@gmail.com", subject, body);

        }
    }
}