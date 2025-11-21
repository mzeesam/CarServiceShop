using CarServiceShop.Shared.Enums;

namespace CarServiceShop.Core.DTOs;

public class AppointmentDto
{
    public int Id { get; set; }
    public string AppointmentNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int VehicleId { get; set; }
    public string VehicleDescription { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public decimal Duration { get; set; }
    public string ServiceType { get; set; } = string.Empty;
    public int? BayId { get; set; }
    public string? BayName { get; set; }
    public int? TechnicianId { get; set; }
    public string? TechnicianName { get; set; }
    public AppointmentStatus Status { get; set; }
    public string CustomerNotes { get; set; } = string.Empty;
    public string InternalNotes { get; set; } = string.Empty;
    public bool ReminderSent { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateAppointmentRequest
{
    public int CustomerId { get; set; }
    public int VehicleId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public decimal Duration { get; set; }
    public string ServiceType { get; set; } = string.Empty;
    public int? BayId { get; set; }
    public int? TechnicianId { get; set; }
    public string CustomerNotes { get; set; } = string.Empty;
    public string InternalNotes { get; set; } = string.Empty;
}

public class UpdateAppointmentRequest
{
    public DateTime AppointmentDate { get; set; }
    public decimal Duration { get; set; }
    public string ServiceType { get; set; } = string.Empty;
    public int? BayId { get; set; }
    public int? TechnicianId { get; set; }
    public AppointmentStatus Status { get; set; }
    public string CustomerNotes { get; set; } = string.Empty;
    public string InternalNotes { get; set; } = string.Empty;
    public bool ReminderSent { get; set; }
}
