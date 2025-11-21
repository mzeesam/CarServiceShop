using CarServiceShop.Shared.Enums;

namespace CarServiceShop.Core.DTOs;

public class WorkOrderDto
{
    public int Id { get; set; }
    public string WorkOrderNumber { get; set; } = string.Empty;
    public int? EstimateId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int VehicleId { get; set; }
    public string VehicleDescription { get; set; } = string.Empty;
    public int? MileageIn { get; set; }
    public int? MileageOut { get; set; }
    public Priority Priority { get; set; }
    public WorkOrderStatus Status { get; set; }
    public DateTime DateOpened { get; set; }
    public DateTime? DateDue { get; set; }
    public DateTime? DateCompleted { get; set; }
    public int? BayId { get; set; }
    public string? BayName { get; set; }
    public int? TechnicianId { get; set; }
    public string? TechnicianName { get; set; }
    public string CustomerComplaint { get; set; } = string.Empty;
    public string DiagnosisNotes { get; set; } = string.Empty;
    public string WorkPerformed { get; set; } = string.Empty;
    public string Recommendations { get; set; } = string.Empty;
    public List<WorkOrderItemDto> WorkOrderItems { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class WorkOrderItemDto
{
    public int Id { get; set; }
    public int WorkOrderId { get; set; }
    public string ItemType { get; set; } = string.Empty;
    public int? ServiceId { get; set; }
    public string? ServiceName { get; set; }
    public int? PartId { get; set; }
    public string? PartName { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Total { get; set; }
    public int? TechnicianId { get; set; }
    public string? TechnicianName { get; set; }
    public decimal? EstimatedHours { get; set; }
    public decimal? ActualHours { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class CreateWorkOrderRequest
{
    public int? EstimateId { get; set; }
    public int CustomerId { get; set; }
    public int VehicleId { get; set; }
    public int? MileageIn { get; set; }
    public Priority Priority { get; set; } = Priority.Normal;
    public DateTime? DateDue { get; set; }
    public int? BayId { get; set; }
    public int? TechnicianId { get; set; }
    public string CustomerComplaint { get; set; } = string.Empty;
    public string DiagnosisNotes { get; set; } = string.Empty;
    public List<CreateWorkOrderItemRequest> WorkOrderItems { get; set; } = new();
}

public class CreateWorkOrderItemRequest
{
    public string ItemType { get; set; } = string.Empty;
    public int? ServiceId { get; set; }
    public int? PartId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int? TechnicianId { get; set; }
    public decimal? EstimatedHours { get; set; }
}

public class UpdateWorkOrderRequest
{
    public int? MileageOut { get; set; }
    public Priority Priority { get; set; }
    public WorkOrderStatus Status { get; set; }
    public DateTime? DateDue { get; set; }
    public DateTime? DateCompleted { get; set; }
    public int? BayId { get; set; }
    public int? TechnicianId { get; set; }
    public string DiagnosisNotes { get; set; } = string.Empty;
    public string WorkPerformed { get; set; } = string.Empty;
    public string Recommendations { get; set; } = string.Empty;
}
