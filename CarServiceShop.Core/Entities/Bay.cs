namespace CarServiceShop.Core.Entities;

public class Bay : BaseEntity
{
    public string BayNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? BayType { get; set; } // General, Alignment, Paint Booth, Wash Bay
    public string Status { get; set; } = "Available"; // Available, Occupied, Maintenance
    public bool IsActive { get; set; } = true;
    public string? Notes { get; set; }

    // Navigation properties
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public virtual ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();
}
