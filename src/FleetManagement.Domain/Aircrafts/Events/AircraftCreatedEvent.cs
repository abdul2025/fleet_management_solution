using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FleetManagement.Domain.CommonEntities;

namespace FleetManagement.Domain.Aircrafts.Events
{
    public sealed record AircraftCreatedEvent(string RegistrationNumber) : IDomainEvent;
}