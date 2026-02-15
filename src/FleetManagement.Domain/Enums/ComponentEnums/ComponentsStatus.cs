namespace FleetManagement.Domain.Enums.ComponentEnums
{
    public enum ComponentsStatus
    {
        Installed = 1,      // Currently installed on the aircraft
        Removed = 2,        // Removed from aircraft but not yet discarded
        Overhauled = 3,     // Component has been serviced/overhauled
        Scrapped = 4,       // Component has been discarded
        InTransit = 5,      // Being moved between locations or aircraft
        OnOrder = 6         // Ordered but not yet received 
    }
}