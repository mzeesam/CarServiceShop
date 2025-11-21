using CarServiceShop.Shared.Enums;

namespace CarServiceShop.Core.DTOs;

public class EstimateDto
{
    public int Id { get; set; }
    public string EstimateNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int VehicleId { get; set; }
    public string VehicleDescription { get; set; } = string.Empty;
    public DateTime? ValidUntil { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    public EstimateStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;
    public List<EstimateItemDto> EstimateItems { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class EstimateItemDto
{
    public int Id { get; set; }
    public int EstimateId { get; set; }
    public string ItemType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Total { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class CreateEstimateRequest
{
    public int CustomerId { get; set; }
    public int VehicleId { get; set; }
    public DateTime? ValidUntil { get; set; }
    public decimal Discount { get; set; }
    public decimal Tax { get; set; }
    public string Notes { get; set; } = string.Empty;
    public List<CreateEstimateItemRequest> EstimateItems { get; set; } = new();
}

public class CreateEstimateItemRequest
{
    public string ItemType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class UpdateEstimateRequest
{
    public DateTime? ValidUntil { get; set; }
    public decimal Discount { get; set; }
    public decimal Tax { get; set; }
    public EstimateStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;
}
