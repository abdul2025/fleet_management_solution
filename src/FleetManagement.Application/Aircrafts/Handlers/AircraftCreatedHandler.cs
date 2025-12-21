using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FleetManagement.Domain.Aircrafts.Events;

namespace FleetManagement.Application.Aircrafts.Handlers
{
    public class AircraftCreatedHandler
    {
        public void Handle(AircraftCreatedEvent @event)
        {
            // Your custom logic here
                    Console.WriteLine($"Aircraft {@event.RegistrationNumber} created at {@event.OccurredOn:O}");
        }
    }
}