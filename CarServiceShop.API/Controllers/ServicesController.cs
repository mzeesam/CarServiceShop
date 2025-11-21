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
public class ServicesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ServicesController> _logger;

    public ServicesController(ApplicationDbContext context, ILogger<ServicesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceDto>>> GetServices()
    {
        try
        {
            var services = await _context.Services
                .Include(s => s.Category)
                .ToListAsync();

            var serviceDtos = services.Select(s => new ServiceDto
            {
                Id = s.Id,
                ServiceCode = s.ServiceCode,
                Name = s.Name,
                Description = s.Description ?? string.Empty,
                CategoryId = s.CategoryId,
                CategoryName = s.Category?.Name,
                StandardHours = s.StandardHours,
                LaborRate = s.LaborRate,
                FlatRate = s.FlatRate,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt
            }).ToList();

            return Ok(serviceDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving services");
            return StatusCode(500, new { message = "An error occurred while retrieving services" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceDto>> GetService(int id)
    {
        try
        {
            var service = await _context.Services
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null)
            {
                return NotFound(new { message = "Service not found" });
            }

            var serviceDto = new ServiceDto
            {
                Id = service.Id,
                ServiceCode = service.ServiceCode,
                Name = service.Name,
                Description = service.Description ?? string.Empty,
                CategoryId = service.CategoryId,
                CategoryName = service.Category?.Name,
                StandardHours = service.StandardHours,
                LaborRate = service.LaborRate,
                FlatRate = service.FlatRate,
                IsActive = service.IsActive,
                CreatedAt = service.CreatedAt
            };

            return Ok(serviceDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving service {Id}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the service" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<ServiceDto>> CreateService([FromBody] CreateServiceRequest request)
    {
        try
        {
            var existingService = await _context.Services.AnyAsync(s => s.ServiceCode == request.ServiceCode);
            if (existingService)
            {
                return BadRequest(new { message = "Service with this service code already exists" });
            }

            var service = new Service
            {
                ServiceCode = request.ServiceCode,
                Name = request.Name,
                Description = request.Description,
                CategoryId = request.CategoryId,
                StandardHours = request.StandardHours,
                LaborRate = request.LaborRate,
                FlatRate = request.FlatRate,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            var createdService = await _context.Services
                .Include(s => s.Category)
                .FirstAsync(s => s.Id == service.Id);

            var serviceDto = new ServiceDto
            {
                Id = createdService.Id,
                ServiceCode = createdService.ServiceCode,
                Name = createdService.Name,
                Description = createdService.Description ?? string.Empty,
                CategoryId = createdService.CategoryId,
                CategoryName = createdService.Category?.Name,
                StandardHours = createdService.StandardHours,
                LaborRate = createdService.LaborRate,
                FlatRate = createdService.FlatRate,
                IsActive = createdService.IsActive,
                CreatedAt = createdService.CreatedAt
            };

            return CreatedAtAction(nameof(GetService), new { id = service.Id }, serviceDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating service");
            return StatusCode(500, new { message = "An error occurred while creating the service" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateService(int id, [FromBody] UpdateServiceRequest request)
    {
        try
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound(new { message = "Service not found" });
            }

            service.Name = request.Name;
            service.Description = request.Description;
            service.CategoryId = request.CategoryId;
            service.StandardHours = request.StandardHours;
            service.LaborRate = request.LaborRate;
            service.FlatRate = request.FlatRate;
            service.IsActive = request.IsActive;
            service.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating service {Id}", id);
            return StatusCode(500, new { message = "An error occurred while updating the service" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteService(int id)
    {
        try
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound(new { message = "Service not found" });
            }

            service.IsDeleted = true;
            service.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting service {Id}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the service" });
        }
    }
}
