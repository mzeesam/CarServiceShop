using CarServiceShop.Shared.Enums;

namespace CarServiceShop.Core.DTOs;

public class VehicleDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string RegistrationNumber { get; set; } = string.Empty;
    public string? VIN { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public VehicleEngineType EngineType { get; set; }
    public int CurrentMileage { get; set; }
    public string? Color { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateVehicleDto
{
    public int CustomerId { get; set; }
    public string RegistrationNumber { get; set; } = string.Empty;
    public string? VIN { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public VehicleEngineType EngineType { get; set; }
    public string? EngineSize { get; set; }
    public string? Transmission { get; set; }
    public string? Color { get; set; }
    public int CurrentMileage { get; set; }
    public string? Notes { get; set; }
}

public class UpdateVehicleDto
{
    public string RegistrationNumber { get; set; } = string.Empty;
    public string? VIN { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public VehicleEngineType EngineType { get; set; }
    public string? EngineSize { get; set; }
    public string? Transmission { get; set; }
    public string? Color { get; set; }
    public int CurrentMileage { get; set; }
    public string? Notes { get; set; }
}
