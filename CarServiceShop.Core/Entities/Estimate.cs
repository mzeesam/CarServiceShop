using CarServiceShop.Shared.Enums;

namespace CarServiceShop.Core.Entities;

public class Estimate : BaseEntity
{
    public string EstimateNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public int VehicleId { get; set; }
    public DateTime ValidUntil { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    public EstimateStatus Status { get; set; }
    public string? TermsAndConditions { get; set; }
    public string? CustomerSignature { get; set; }
    public string? Notes { get; set; }

    // Navigation properties
    public virtual Customer Customer { get; set; } = null!;
    public virtual Vehicle Vehicle { get; set; } = null!;
    public virtual ICollection<EstimateItem> EstimateItems { get; set; } = new List<EstimateItem>();
    public virtual WorkOrder? WorkOrder { get; set; }
}
