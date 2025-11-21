namespace CarServiceShop.Shared.Enums;

public enum WorkOrderStatus
{
    Open = 1,
    Assigned = 2,
    InProgress = 3,
    WaitingForParts = 4,
    WaitingForApproval = 5,
    QualityCheck = 6,
    ReadyForPickup = 7,
    Completed = 8,
    OnHold = 9,
    Cancelled = 10
}
