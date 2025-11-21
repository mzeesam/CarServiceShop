namespace CarServiceShop.Core.Entities;

public class WorkOrderItem : BaseEntity
{
    public int WorkOrderId { get; set; }
    public string ItemType { get; set; } = string.Empty; // Labor, Part
    public int? ServiceId { get; set; }
    public int? PartId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Total { get; set; }
    public int? TechnicianId { get; set; }
    public decimal? EstimatedHours { get; set; }
    public decimal? ActualHours { get; set; }
    public string Status { get; set; } = string.Empty;

    // Navigation properties
    public virtual WorkOrder WorkOrder { get; set; } = null!;
    public virtual Service? Service { get; set; }
    public virtual Part? Part { get; set; }
    public virtual User? Technician { get; set; }
}
