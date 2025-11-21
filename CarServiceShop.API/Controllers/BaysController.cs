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
public class BaysController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BaysController> _logger;

    public BaysController(ApplicationDbContext context, ILogger<BaysController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BayDto>>> GetBays()
    {
        try
        {
            var bays = await _context.Bays.ToListAsync();

            var bayDtos = bays.Select(b => new BayDto
            {
                Id = b.Id,
                BayNumber = b.BayNumber,
                Name = b.Name,
                BayType = b.BayType,
                Status = b.Status,
                IsActive = b.IsActive,
                Description = b.Notes,
                CreatedAt = b.CreatedAt
            }).ToList();

            return Ok(bayDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bays");
            return StatusCode(500, new { message = "An error occurred while retrieving bays" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BayDto>> GetBay(int id)
    {
        try
        {
            var bay = await _context.Bays.FindAsync(id);

            if (bay == null)
            {
                return NotFound(new { message = "Bay not found" });
            }

            var bayDto = new BayDto
            {
                Id = bay.Id,
                BayNumber = bay.BayNumber,
                Name = bay.Name,
                BayType = bay.BayType,
                Status = bay.Status,
                IsActive = bay.IsActive,
                Description = bay.Notes,
                CreatedAt = bay.CreatedAt
            };

            return Ok(bayDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bay {Id}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the bay" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<BayDto>> CreateBay([FromBody] CreateBayRequest request)
    {
        try
        {
            var existingBay = await _context.Bays.AnyAsync(b => b.BayNumber == request.BayNumber);
            if (existingBay)
            {
                return BadRequest(new { message = "Bay with this bay number already exists" });
            }

            var bay = new Bay
            {
                BayNumber = request.BayNumber,
                Name = request.Name,
                BayType = request.BayType,
                Status = "Available",
                Notes = request.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Bays.Add(bay);
            await _context.SaveChangesAsync();

            var bayDto = new BayDto
            {
                Id = bay.Id,
                BayNumber = bay.BayNumber,
                Name = bay.Name,
                BayType = bay.BayType,
                Status = bay.Status,
                IsActive = bay.IsActive,
                Description = bay.Notes,
                CreatedAt = bay.CreatedAt
            };

            return CreatedAtAction(nameof(GetBay), new { id = bay.Id }, bayDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating bay");
            return StatusCode(500, new { message = "An error occurred while creating the bay" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBay(int id, [FromBody] UpdateBayRequest request)
    {
        try
        {
            var bay = await _context.Bays.FindAsync(id);
            if (bay == null)
            {
                return NotFound(new { message = "Bay not found" });
            }

            bay.Name = request.Name;
            bay.BayType = request.BayType;
            bay.Status = request.Status;
            bay.Notes = request.Description;
            bay.IsActive = request.IsActive;
            bay.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating bay {Id}", id);
            return StatusCode(500, new { message = "An error occurred while updating the bay" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBay(int id)
    {
        try
        {
            var bay = await _context.Bays.FindAsync(id);
            if (bay == null)
            {
                return NotFound(new { message = "Bay not found" });
            }

            bay.IsDeleted = true;
            bay.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting bay {Id}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the bay" });
        }
    }
}
