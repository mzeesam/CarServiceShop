using CarServiceShop.Shared.Enums;

namespace CarServiceShop.Core.Entities;

public class Vehicle : BaseEntity
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
    public string? FuelType { get; set; }
    public string? BodyType { get; set; }
    public string? InsuranceDetails { get; set; }
    public DateTime? NextServiceDueDate { get; set; }
    public int? NextServiceDueMileage { get; set; }
    public string? Notes { get; set; }

    // Navigation properties
    public virtual Customer Customer { get; set; } = null!;
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public virtual ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();
}
