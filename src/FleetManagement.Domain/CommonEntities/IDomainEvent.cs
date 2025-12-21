using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagement.Domain.CommonEntities
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
}