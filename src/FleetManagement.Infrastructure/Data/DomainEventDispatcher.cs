using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FleetManagement.Domain.CommonEntities;
using FleetManagement.Domain.Aircrafts.Events;
using FleetManagement.Application.Aircrafts.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace FleetManagement.Infrastructure.Data
{
    public class DomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // Dispatch events for a single entity
        public Task Dispatch(BaseEntity entity)
        {
            foreach (var domainEvent in entity.DomainEvents)
            {
                // Fire-and-forget: run in background
                _ = Task.Run(async () =>
                {
                    switch (domainEvent)
                    {
                        case AircraftCreatedEvent aircraftCreatedEvent:
                            var handler = _serviceProvider.GetRequiredService<AircraftCreatedHandler>();
                            await handler.Handle(aircraftCreatedEvent);
                            break;

                        // Add other events here
                    }
                });
            }

            entity.ClearDomainEvents();

            // Return completed task so it can be awaited if needed
            return Task.CompletedTask;
        }

        // Dispatch events for multiple entities
        public Task DispatchAll(IEnumerable<BaseEntity> entities)
        {
            foreach (var entity in entities)
            {
                _ = Dispatch(entity); // fire-and-forget per entity
            }

            return Task.CompletedTask;
        }
    }
}
