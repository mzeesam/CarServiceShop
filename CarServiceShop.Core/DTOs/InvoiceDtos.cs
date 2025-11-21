using CarServiceShop.Shared.Enums;

namespace CarServiceShop.Core.DTOs;

public class InvoiceDto
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public int WorkOrderId { get; set; }
    public string WorkOrderNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public DateTime? DueDate { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal Balance { get; set; }
    public PaymentStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;
    public List<PaymentDto> Payments { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class PaymentDto
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string Reference { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateInvoiceRequest
{
    public int WorkOrderId { get; set; }
    public DateTime? DueDate { get; set; }
    public decimal Discount { get; set; }
    public decimal Tax { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class CreatePaymentRequest
{
    public int InvoiceId { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string Reference { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public class UpdatePaymentStatusRequest
{
    public PaymentStatus Status { get; set; }
}
