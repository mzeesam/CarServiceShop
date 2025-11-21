using CarServiceShop.Shared.Enums;

namespace CarServiceShop.Core.Entities;

public class Appointment : BaseEntity
{
    public string AppointmentNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public int VehicleId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public int EstimatedDuration { get; set; } // in minutes
    public AppointmentStatus Status { get; set; }
    public int? BayId { get; set; }
    public int? TechnicianId { get; set; }
    public string? ServiceTypeRequested { get; set; }
    public string? CustomerNotes { get; set; }
    public string? InternalNotes { get; set; }
    public bool ReminderSent { get; set; }

    // Navigation properties
    public virtual Customer Customer { get; set; } = null!;
    public virtual Vehicle Vehicle { get; set; } = null!;
    public virtual Bay? Bay { get; set; }
    public virtual User? Technician { get; set; }
}
