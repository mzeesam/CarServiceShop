using CarServiceShop.Shared.Enums;

namespace CarServiceShop.Core.Entities;

public class WorkOrder : BaseEntity
{
    public string WorkOrderNumber { get; set; } = string.Empty;
    public int? EstimateId { get; set; }
    public int CustomerId { get; set; }
    public int VehicleId { get; set; }
    public int MileageIn { get; set; }
    public int? MileageOut { get; set; }
    public DateTime DateOpened { get; set; } = DateTime.UtcNow;
    public DateTime? DateDue { get; set; }
    public DateTime? DateCompleted { get; set; }
    public Priority Priority { get; set; }
    public WorkOrderStatus Status { get; set; }
    public int? BayId { get; set; }
    public int? TechnicianId { get; set; }
    public string? CustomerComplaint { get; set; }
    public string? DiagnosisNotes { get; set; }
    public string? WorkPerformed { get; set; }
    public string? Recommendations { get; set; }
    public string? TechnicianSignOff { get; set; }
    public string? QualityCheckSignOff { get; set; }
    public string? CustomerSignOff { get; set; }

    // Navigation properties
    public virtual Estimate? Estimate { get; set; }
    public virtual Customer Customer { get; set; } = null!;
    public virtual Vehicle Vehicle { get; set; } = null!;
    public virtual Bay? Bay { get; set; }
    public virtual User? Technician { get; set; }
    public virtual ICollection<WorkOrderItem> WorkOrderItems { get; set; } = new List<WorkOrderItem>();
    public virtual Invoice? Invoice { get; set; }
}
