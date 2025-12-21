using System;
using System.Linq;
using System.Threading.Tasks;
using FleetManagement.Domain.CommonEntities;
using FleetManagement.Domain.Aircrafts.Events; // ✅ Add this

namespace FleetManagement.Infrastructure.Data
{
    public class DomainEventDispatcher
    {
        public void Dispatch(BaseEntity entity)
        {
            foreach (var domainEvent in entity.DomainEvents)
            {
                switch (domainEvent)
                {
                    case AircraftCreatedEvent aircraftCreatedEvent:
                        // Call your handler
                        new FleetManagement.Application.Aircrafts.Handlers.AircraftCreatedHandler()
                            .Handle(aircraftCreatedEvent);
                        break;

                    // Add other events here
                }
            }

            entity.ClearDomainEvents();
        }

        public void DispatchAll(System.Collections.Generic.IEnumerable<BaseEntity> entities)
        {
            foreach (var entity in entities)
                Dispatch(entity);
        }
    }
}
