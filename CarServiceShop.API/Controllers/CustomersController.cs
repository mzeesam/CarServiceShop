using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarServiceShop.Core.DTOs;
using CarServiceShop.Core.Entities;
using CarServiceShop.Infrastructure.Data;

namespace CarServiceShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(ApplicationDbContext context, ILogger<CustomersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
    {
        try
        {
            var customers = await _context.Customers
                .Select(c => new CustomerDto
                {
                    Id = c.Id,
                    CustomerNumber = c.CustomerNumber,
                    CustomerType = c.CustomerType,
                    Name = c.Name,
                    CompanyName = c.CompanyName,
                    Email = c.Email,
                    Phone = c.Phone,
                    SecondaryPhone = c.SecondaryPhone,
                    Address = c.Address,
                    City = c.City,
                    State = c.State,
                    ZipCode = c.ZipCode,
                    TaxNumber = c.TaxNumber,
                    CreditLimit = c.CreditLimit,
                    IsActive = c.IsActive,
                    Notes = c.Notes,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();

            return Ok(customers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customers");
            return StatusCode(500, new { message = "An error occurred while retrieving customers" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
    {
        try
        {
            var customer = await _context.Customers
                .Where(c => c.Id == id)
                .Select(c => new CustomerDto
                {
                    Id = c.Id,
                    CustomerNumber = c.CustomerNumber,
                    CustomerType = c.CustomerType,
                    Name = c.Name,
                    CompanyName = c.CompanyName,
                    Email = c.Email,
                    Phone = c.Phone,
                    SecondaryPhone = c.SecondaryPhone,
                    Address = c.Address,
                    City = c.City,
                    State = c.State,
                    ZipCode = c.ZipCode,
                    TaxNumber = c.TaxNumber,
                    CreditLimit = c.CreditLimit,
                    IsActive = c.IsActive,
                    Notes = c.Notes,
                    CreatedAt = c.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (customer == null)
            {
                return NotFound(new { message = "Customer not found" });
            }

            return Ok(customer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customer {Id}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the customer" });
        }
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdministrator,ShopManager,ServiceAdvisor,Cashier")]
    public async Task<ActionResult<CustomerDto>> CreateCustomer([FromBody] CreateCustomerDto dto)
    {
        try
        {
            // Generate customer number
            var lastCustomer = await _context.Customers
                .OrderByDescending(c => c.Id)
                .FirstOrDefaultAsync();
            
            var customerNumber = $"CUST-{(lastCustomer?.Id + 1 ?? 1):D6}";

            var customer = new Customer
            {
                CustomerNumber = customerNumber,
                CustomerType = dto.CustomerType,
                Name = dto.Name,
                CompanyName = dto.CompanyName,
                Email = dto.Email,
                Phone = dto.Phone,
                SecondaryPhone = dto.SecondaryPhone,
                Address = dto.Address,
                City = dto.City,
                State = dto.State,
                ZipCode = dto.ZipCode,
                TaxNumber = dto.TaxNumber,
                CreditLimit = dto.CreditLimit,
                IsActive = true,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var result = new CustomerDto
            {
                Id = customer.Id,
                CustomerNumber = customer.CustomerNumber,
                CustomerType = customer.CustomerType,
                Name = customer.Name,
                CompanyName = customer.CompanyName,
                Email = customer.Email,
                Phone = customer.Phone,
                SecondaryPhone = customer.SecondaryPhone,
                Address = customer.Address,
                City = customer.City,
                State = customer.State,
                ZipCode = customer.ZipCode,
                TaxNumber = customer.TaxNumber,
                CreditLimit = customer.CreditLimit,
                IsActive = customer.IsActive,
                Notes = customer.Notes,
                CreatedAt = customer.CreatedAt
            };

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating customer");
            return StatusCode(500, new { message = "An error occurred while creating the customer" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "SuperAdministrator,ShopManager,ServiceAdvisor,Cashier")]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerDto dto)
    {
        try
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound(new { message = "Customer not found" });
            }

            customer.CustomerType = dto.CustomerType;
            customer.Name = dto.Name;
            customer.CompanyName = dto.CompanyName;
            customer.Email = dto.Email;
            customer.Phone = dto.Phone;
            customer.SecondaryPhone = dto.SecondaryPhone;
            customer.Address = dto.Address;
            customer.City = dto.City;
            customer.State = dto.State;
            customer.ZipCode = dto.ZipCode;
            customer.TaxNumber = dto.TaxNumber;
            customer.CreditLimit = dto.CreditLimit;
            customer.IsActive = dto.IsActive;
            customer.Notes = dto.Notes;
            customer.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating customer {Id}", id);
            return StatusCode(500, new { message = "An error occurred while updating the customer" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdministrator,ShopManager")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        try
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound(new { message = "Customer not found" });
            }

            // Soft delete
            customer.IsDeleted = true;
            customer.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting customer {Id}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the customer" });
        }
    }
}
