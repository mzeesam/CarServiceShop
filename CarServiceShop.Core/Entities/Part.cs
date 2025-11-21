namespace CarServiceShop.Core.Entities;

public class Part : BaseEntity
{
    public string PartNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? CategoryId { get; set; }
    public int? SupplierId { get; set; }
    public decimal CostPrice { get; set; }
    public decimal RetailPrice { get; set; }
    public decimal? WholesalePrice { get; set; }
    public int QuantityOnHand { get; set; }
    public int MinimumStock { get; set; }
    public int? ReorderQuantity { get; set; }
    public string? Location { get; set; }
    public string? Barcode { get; set; }
    public bool IsActive { get; set; } = true;
    public string? ImageUrl { get; set; }
    public string? Notes { get; set; }

    // Navigation properties
    public virtual Category? Category { get; set; }
    public virtual Supplier? Supplier { get; set; }
    public virtual ICollection<WorkOrderItem> WorkOrderItems { get; set; } = new List<WorkOrderItem>();
}
