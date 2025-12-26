using FleetManagement.Application.Aircrafts.DTOs;
using FleetManagement.Application.Aircrafts.Interfaces;
using FleetManagement.Domain.Aircrafts.Entities;
using FleetManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Infrastructure.Services.Aircrafts
{
    public class AircraftService : IAircraftService
    {
        private readonly ApplicationDbContext _context;

        public AircraftService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AircraftDto>> GetAllAsync()
        {
            return await _context.Aircrafts
                .Include(a => a.AircraftSpecification)
                .Select(a => new AircraftDto
                {
                    Id = a.Id,
                    RegistrationNumber = a.RegistrationNumber,
                    SerialNumber = a.SerialNumber,
                    Model = a.Model,
                    Manufacturer = a.Manufacturer,
                    YearOfManufacture = a.YearOfManufacture, // keep as nullable in DTO if DB is nullable
                    Status = a.Status,
                    Specification = a.AircraftSpecification == null ? null : new AircraftSpecificationDto
                    {
                        BasedStation = a.AircraftSpecification.BasedStation,
                        SeatingCapacity = a.AircraftSpecification.SeatingCapacity,
                        MaxTakeoffWeight = a.AircraftSpecification.MaxTakeoffWeight,
                        MaxLandingWeight = a.AircraftSpecification.MaxLandingWeight,
                        WeightUnit = a.AircraftSpecification.WeightUnit
                    }
                })
                .ToListAsync();
        }

        public async Task<AircraftDto?> GetByIdAsync(int id)
        {
            var aircraft = await _context.Aircrafts
                .Include(a => a.AircraftSpecification)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (aircraft == null) return null;

            return new AircraftDto
            {
                Id = aircraft.Id,
                RegistrationNumber = aircraft.RegistrationNumber,
                SerialNumber = aircraft.SerialNumber,
                Model = aircraft.Model,
                Manufacturer = aircraft.Manufacturer,
                YearOfManufacture = aircraft.YearOfManufacture,
                Status = aircraft.Status,
                Specification = new AircraftSpecificationDto
                {
                    BasedStation = aircraft.AircraftSpecification.BasedStation,
                    SeatingCapacity = aircraft.AircraftSpecification.SeatingCapacity,
                    MaxTakeoffWeight = aircraft.AircraftSpecification.MaxTakeoffWeight,
                    MaxLandingWeight = aircraft.AircraftSpecification.MaxLandingWeight,
                    WeightUnit = aircraft.AircraftSpecification.WeightUnit
                }
            };
        }

        public async Task<AircraftDto> CreateAsync(AircraftDto dto)
        {
            // Use factory method for core properties
            var aircraft = Aircraft.Create(dto.RegistrationNumber, dto.Model, dto.Manufacturer, dto.Status);

            // Set optional/additional properties
            aircraft.SerialNumber = dto.SerialNumber;
            aircraft.YearOfManufacture = dto.YearOfManufacture;
            aircraft.AircraftSpecification = new AircraftSpecification
            {
                BasedStation = dto.Specification.BasedStation,
                SeatingCapacity = dto.Specification.SeatingCapacity,
                MaxTakeoffWeight = dto.Specification.MaxTakeoffWeight,
                MaxLandingWeight = dto.Specification.MaxLandingWeight,
                WeightUnit = dto.Specification.WeightUnit
            };

            _context.Aircrafts.Add(aircraft);
            await _context.SaveChangesAsync();

            dto.Id = aircraft.Id;
            return dto;
        }

        public async Task<AircraftDto?> UpdateAsync(int id, AircraftDto dto)
        {
            var aircraft = await _context.Aircrafts
                .Include(a => a.AircraftSpecification)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (aircraft == null) return null;

            aircraft.RegistrationNumber = dto.RegistrationNumber;
            aircraft.SerialNumber = dto.SerialNumber;
            aircraft.Model = dto.Model;
            aircraft.Manufacturer = dto.Manufacturer;
            aircraft.YearOfManufacture = dto.YearOfManufacture;
            aircraft.Status = dto.Status;

            var spec = aircraft.AircraftSpecification;
            spec.BasedStation = dto.Specification.BasedStation;
            spec.SeatingCapacity = dto.Specification.SeatingCapacity;
            spec.MaxTakeoffWeight = dto.Specification.MaxTakeoffWeight;
            spec.MaxLandingWeight = dto.Specification.MaxLandingWeight;
            spec.WeightUnit = dto.Specification.WeightUnit;

            await _context.SaveChangesAsync();
            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var aircraft = await _context.Aircrafts.FindAsync(id);
            if (aircraft == null) return false;

            _context.Aircrafts.Remove(aircraft);
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<IEnumerable<AircraftDto>> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
                return await GetAllAsync(); // return full list if query is too short

            query = query.Trim().ToLower();

            var aircrafts = await _context.Aircrafts
                .Include(a => a.AircraftSpecification)
                .Where(a =>
                    a.RegistrationNumber.ToLower().Contains(query) ||
                    a.Model.ToLower().Contains(query) ||
                    a.SerialNumber.ToLower().Contains(query) ||
                    a.Manufacturer.ToString().ToLower().Contains(query) ||
                    a.Status.ToString().ToLower().Contains(query) ||
                    (a.YearOfManufacture.HasValue && a.YearOfManufacture.Value.ToString().Contains(query)) ||
                    (a.AircraftSpecification != null && a.AircraftSpecification.BasedStation.ToLower().Contains(query))
                )
                .Select(a => new AircraftDto
                {
                    Id = a.Id,
                    RegistrationNumber = a.RegistrationNumber,
                    SerialNumber = a.SerialNumber,
                    Model = a.Model,
                    Manufacturer = a.Manufacturer,
                    YearOfManufacture = a.YearOfManufacture,
                    Status = a.Status,
                    Specification = a.AircraftSpecification == null ? null : new AircraftSpecificationDto
                    {
                        BasedStation = a.AircraftSpecification.BasedStation,
                        SeatingCapacity = a.AircraftSpecification.SeatingCapacity,
                        MaxTakeoffWeight = a.AircraftSpecification.MaxTakeoffWeight,
                        MaxLandingWeight = a.AircraftSpecification.MaxLandingWeight,
                        WeightUnit = a.AircraftSpecification.WeightUnit
                    }
                })
                .ToListAsync();

            return aircrafts;
        }



        public async Task<ImportResultDto> ImportFromJsonAsync(IEnumerable<AircraftDto> dtos)
        {
            var result = new ImportResultDto();

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var dto in dtos)
                {
                    // Validate required fields
                    if (string.IsNullOrWhiteSpace(dto.RegistrationNumber))
                    {
                        result.Errors.Add($"Missing RegistrationNumber for Model: {dto.Model}");
                        continue;
                    }

                    // Optional: check if aircraft already exists
                    if (await _context.Aircrafts.AnyAsync(a => a.RegistrationNumber == dto.RegistrationNumber))
                    {
                        result.Errors.Add($"Aircraft with RegistrationNumber {dto.RegistrationNumber} already exists.");
                        continue;
                    }

                    // Map DTO to domain entity
                    var aircraft = Aircraft.Create(dto.RegistrationNumber, dto.Model, dto.Manufacturer, dto.Status);
                    aircraft.SerialNumber = dto.SerialNumber;
                    aircraft.YearOfManufacture = dto.YearOfManufacture;

                    aircraft.AircraftSpecification = new AircraftSpecification
                    {
                        BasedStation = dto.Specification.BasedStation,
                        SeatingCapacity = dto.Specification.SeatingCapacity,
                        MaxTakeoffWeight = dto.Specification.MaxTakeoffWeight,
                        MaxLandingWeight = dto.Specification.MaxLandingWeight,
                        WeightUnit = dto.Specification.WeightUnit
                    };

                    _context.Aircrafts.Add(aircraft);
                    result.SuccessCount++;
                }

                if (result.SuccessCount > 0)
                {
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result.Errors.Add("Import failed: " + ex.Message);
            }

            return result;
        }

    }
}
