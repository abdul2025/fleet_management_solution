using FleetManagement.Application.Aircrafts.DTOs;

namespace FleetManagement.Application.Aircrafts.Interfaces
{
    public interface IAircraftService
    {
        Task<IEnumerable<AircraftDto>> GetAllAsync();
        Task<AircraftDto?> GetByIdAsync(int id);
        Task<AircraftDto> CreateAsync(AircraftDto dto);
        Task<AircraftDto?> UpdateAsync(int id, AircraftDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
