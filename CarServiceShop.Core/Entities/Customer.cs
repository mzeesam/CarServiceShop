using CarServiceShop.Shared.Enums;

namespace CarServiceShop.Core.Entities;

public class Customer : BaseEntity
{
    public string CustomerNumber { get; set; } = string.Empty;
    public CustomerType CustomerType { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? SecondaryPhone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? TaxNumber { get; set; }
    public string? PreferredContactMethod { get; set; }
    public string? ReferralSource { get; set; }
    public decimal CreditLimit { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Notes { get; set; }

    // Navigation properties
    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public virtual ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
