using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarServiceShop.Core.DTOs;
using CarServiceShop.Core.Entities;
using CarServiceShop.Infrastructure.Data;

namespace CarServiceShop.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PartsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PartsController> _logger;

    public PartsController(ApplicationDbContext context, ILogger<PartsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PartDto>>> GetParts()
    {
        try
        {
            var parts = await _context.Parts
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .ToListAsync();

            var partDtos = parts.Select(p => new PartDto
            {
                Id = p.Id,
                PartNumber = p.PartNumber,
                Name = p.Name,
                Description = p.Description ?? string.Empty,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name,
                SupplierId = p.SupplierId,
                SupplierName = p.Supplier?.CompanyName,
                CostPrice = p.CostPrice,
                RetailPrice = p.RetailPrice,
                WholesalePrice = p.WholesalePrice,
                QuantityOnHand = p.QuantityOnHand,
                MinimumStock = p.MinimumStock,
                ReorderQuantity = p.ReorderQuantity ?? 0,
                Location = p.Location ?? string.Empty,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt
            }).ToList();

            return Ok(partDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving parts");
            return StatusCode(500, new { message = "An error occurred while retrieving parts" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PartDto>> GetPart(int id)
    {
        try
        {
            var part = await _context.Parts
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (part == null)
            {
                return NotFound(new { message = "Part not found" });
            }

            var partDto = new PartDto
            {
                Id = part.Id,
                PartNumber = part.PartNumber,
                Name = part.Name,
                Description = part.Description ?? string.Empty,
                CategoryId = part.CategoryId,
                CategoryName = part.Category?.Name,
                SupplierId = part.SupplierId,
                SupplierName = part.Supplier?.CompanyName,
                CostPrice = part.CostPrice,
                RetailPrice = part.RetailPrice,
                WholesalePrice = part.WholesalePrice,
                QuantityOnHand = part.QuantityOnHand,
                MinimumStock = part.MinimumStock,
                ReorderQuantity = part.ReorderQuantity ?? 0,
                Location = part.Location ?? string.Empty,
                IsActive = part.IsActive,
                CreatedAt = part.CreatedAt
            };

            return Ok(partDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving part {Id}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the part" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<PartDto>> CreatePart([FromBody] CreatePartRequest request)
    {
        try
        {
            var existingPart = await _context.Parts.AnyAsync(p => p.PartNumber == request.PartNumber);
            if (existingPart)
            {
                return BadRequest(new { message = "Part with this part number already exists" });
            }

            var part = new Part
            {
                PartNumber = request.PartNumber,
                Name = request.Name,
                Description = request.Description,
                CategoryId = request.CategoryId,
                SupplierId = request.SupplierId,
                CostPrice = request.CostPrice,
                RetailPrice = request.RetailPrice,
                WholesalePrice = request.WholesalePrice,
                QuantityOnHand = request.QuantityOnHand,
                MinimumStock = request.MinimumStock,
                ReorderQuantity = request.ReorderQuantity,
                Location = request.Location,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Parts.Add(part);
            await _context.SaveChangesAsync();

            var createdPart = await _context.Parts
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstAsync(p => p.Id == part.Id);

            var partDto = new PartDto
            {
                Id = createdPart.Id,
                PartNumber = createdPart.PartNumber,
                Name = createdPart.Name,
                Description = createdPart.Description ?? string.Empty,
                CategoryId = createdPart.CategoryId,
                CategoryName = createdPart.Category?.Name,
                SupplierId = createdPart.SupplierId,
                SupplierName = createdPart.Supplier?.CompanyName,
                CostPrice = createdPart.CostPrice,
                RetailPrice = createdPart.RetailPrice,
                WholesalePrice = createdPart.WholesalePrice,
                QuantityOnHand = createdPart.QuantityOnHand,
                MinimumStock = createdPart.MinimumStock,
                ReorderQuantity = createdPart.ReorderQuantity ?? 0,
                Location = createdPart.Location ?? string.Empty,
                IsActive = createdPart.IsActive,
                CreatedAt = createdPart.CreatedAt
            };

            return CreatedAtAction(nameof(GetPart), new { id = part.Id }, partDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating part");
            return StatusCode(500, new { message = "An error occurred while creating the part" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePart(int id, [FromBody] UpdatePartRequest request)
    {
        try
        {
            var part = await _context.Parts.FindAsync(id);
            if (part == null)
            {
                return NotFound(new { message = "Part not found" });
            }

            part.Name = request.Name;
            part.Description = request.Description;
            part.CategoryId = request.CategoryId;
            part.SupplierId = request.SupplierId;
            part.CostPrice = request.CostPrice;
            part.RetailPrice = request.RetailPrice;
            part.WholesalePrice = request.WholesalePrice;
            part.QuantityOnHand = request.QuantityOnHand;
            part.MinimumStock = request.MinimumStock;
            part.ReorderQuantity = request.ReorderQuantity;
            part.Location = request.Location;
            part.IsActive = request.IsActive;
            part.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating part {Id}", id);
            return StatusCode(500, new { message = "An error occurred while updating the part" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePart(int id)
    {
        try
        {
            var part = await _context.Parts.FindAsync(id);
            if (part == null)
            {
                return NotFound(new { message = "Part not found" });
            }

            part.IsDeleted = true;
            part.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting part {Id}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the part" });
        }
    }
}
