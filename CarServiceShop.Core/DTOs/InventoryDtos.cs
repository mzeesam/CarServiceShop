namespace CarServiceShop.Core.DTOs;

public class PartDto
{
    public int Id { get; set; }
    public string PartNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public int? SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public decimal CostPrice { get; set; }
    public decimal RetailPrice { get; set; }
    public decimal? WholesalePrice { get; set; }
    public int QuantityOnHand { get; set; }
    public int MinimumStock { get; set; }
    public int ReorderQuantity { get; set; }
    public string Location { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreatePartRequest
{
    public string PartNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    public int? SupplierId { get; set; }
    public decimal CostPrice { get; set; }
    public decimal RetailPrice { get; set; }
    public decimal? WholesalePrice { get; set; }
    public int QuantityOnHand { get; set; }
    public int MinimumStock { get; set; }
    public int ReorderQuantity { get; set; }
    public string Location { get; set; } = string.Empty;
}

public class UpdatePartRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    public int? SupplierId { get; set; }
    public decimal CostPrice { get; set; }
    public decimal RetailPrice { get; set; }
    public decimal? WholesalePrice { get; set; }
    public int QuantityOnHand { get; set; }
    public int MinimumStock { get; set; }
    public int ReorderQuantity { get; set; }
    public string Location { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class ServiceDto
{
    public int Id { get; set; }
    public string ServiceCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public decimal StandardHours { get; set; }
    public decimal LaborRate { get; set; }
    public decimal? FlatRate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateServiceRequest
{
    public string ServiceCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    public decimal StandardHours { get; set; }
    public decimal LaborRate { get; set; }
    public decimal? FlatRate { get; set; }
}

public class UpdateServiceRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    public decimal StandardHours { get; set; }
    public decimal LaborRate { get; set; }
    public decimal? FlatRate { get; set; }
    public bool IsActive { get; set; }
}

public class BayDto
{
    public int Id { get; set; }
    public string BayNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string BayType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateBayRequest
{
    public string BayNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string BayType { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdateBayRequest
{
    public string Name { get; set; } = string.Empty;
    public string BayType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class SupplierDto
{
    public int Id { get; set; }
    public string SupplierNumber { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? SecondaryPhone { get; set; }
    public string Address { get; set; } = string.Empty;
    public string? TaxNumber { get; set; }
    public string PaymentTerms { get; set; } = string.Empty;
    public decimal CreditLimit { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateSupplierRequest
{
    public string CompanyName { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? SecondaryPhone { get; set; }
    public string Address { get; set; } = string.Empty;
    public string? TaxNumber { get; set; }
    public string PaymentTerms { get; set; } = string.Empty;
    public decimal CreditLimit { get; set; }
}

public class UpdateSupplierRequest
{
    public string CompanyName { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? SecondaryPhone { get; set; }
    public string Address { get; set; } = string.Empty;
    public string? TaxNumber { get; set; }
    public string PaymentTerms { get; set; } = string.Empty;
    public decimal CreditLimit { get; set; }
    public bool IsActive { get; set; }
}
