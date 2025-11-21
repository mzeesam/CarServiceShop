namespace CarServiceShop.Core.Entities;

public class Supplier : BaseEntity
{
    public string SupplierNumber { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string? ContactPerson { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? TaxNumber { get; set; }
    public string? PaymentTerms { get; set; }
    public string? BankDetails { get; set; }
    public decimal CreditLimit { get; set; }
    public int? Rating { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Notes { get; set; }

    // Navigation properties
    public virtual ICollection<Part> Parts { get; set; } = new List<Part>();
}
