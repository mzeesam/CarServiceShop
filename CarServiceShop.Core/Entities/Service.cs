namespace CarServiceShop.Core.Entities;

public class Service : BaseEntity
{
    public string ServiceCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? CategoryId { get; set; }
    public decimal StandardHours { get; set; }
    public decimal LaborRate { get; set; }
    public decimal? FlatRate { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Notes { get; set; }

    // Navigation properties
    public virtual Category? Category { get; set; }
    public virtual ICollection<WorkOrderItem> WorkOrderItems { get; set; } = new List<WorkOrderItem>();
}
