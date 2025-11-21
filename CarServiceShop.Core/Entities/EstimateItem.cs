namespace CarServiceShop.Core.Entities;

public class EstimateItem : BaseEntity
{
    public int EstimateId { get; set; }
    public string ItemType { get; set; } = string.Empty; // Labor, Part, Sublet
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Total { get; set; }
    public string? Notes { get; set; }

    // Navigation properties
    public virtual Estimate Estimate { get; set; } = null!;
}
