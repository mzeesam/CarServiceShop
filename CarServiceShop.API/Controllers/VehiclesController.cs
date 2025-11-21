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
public class VehiclesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<VehiclesController> _logger;

    public VehiclesController(ApplicationDbContext context, ILogger<VehiclesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VehicleDto>>> GetVehicles([FromQuery] int? customerId = null)
    {
        try
        {
            var query = _context.Vehicles.Include(v => v.Customer).AsQueryable();

            if (customerId.HasValue)
            {
                query = query.Where(v => v.CustomerId == customerId.Value);
            }

            var vehicles = await query
                .Select(v => new VehicleDto
                {
                    Id = v.Id,
                    CustomerId = v.CustomerId,
                    CustomerName = v.Customer.Name,
                    RegistrationNumber = v.RegistrationNumber,
                    VIN = v.VIN,
                    Make = v.Make,
                    Model = v.Model,
                    Year = v.Year,
                    EngineType = v.EngineType,
                    CurrentMileage = v.CurrentMileage,
                    Color = v.Color,
                    CreatedAt = v.CreatedAt
                })
                .ToListAsync();

            return Ok(vehicles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving vehicles");
            return StatusCode(500, new { message = "An error occurred while retrieving vehicles" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VehicleDto>> GetVehicle(int id)
    {
        try
        {
            var vehicle = await _context.Vehicles
                .Include(v => v.Customer)
                .Where(v => v.Id == id)
                .Select(v => new VehicleDto
                {
                    Id = v.Id,
                    CustomerId = v.CustomerId,
                    CustomerName = v.Customer.Name,
                    RegistrationNumber = v.RegistrationNumber,
                    VIN = v.VIN,
                    Make = v.Make,
                    Model = v.Model,
                    Year = v.Year,
                    EngineType = v.EngineType,
                    CurrentMileage = v.CurrentMileage,
                    Color = v.Color,
                    CreatedAt = v.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (vehicle == null)
            {
                return NotFound(new { message = "Vehicle not found" });
            }

            return Ok(vehicle);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving vehicle {Id}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the vehicle" });
        }
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdministrator,ShopManager,ServiceAdvisor,Cashier")]
    public async Task<ActionResult<VehicleDto>> CreateVehicle([FromBody] CreateVehicleDto dto)
    {
        try
        {
            // Verify customer exists
            var customerExists = await _context.Customers.AnyAsync(c => c.Id == dto.CustomerId);
            if (!customerExists)
            {
                return BadRequest(new { message = "Customer not found" });
            }

            var vehicle = new Vehicle
            {
                CustomerId = dto.CustomerId,
                RegistrationNumber = dto.RegistrationNumber,
                VIN = dto.VIN,
                Make = dto.Make,
                Model = dto.Model,
                Year = dto.Year,
                EngineType = dto.EngineType,
                EngineSize = dto.EngineSize,
                Transmission = dto.Transmission,
                Color = dto.Color,
                CurrentMileage = dto.CurrentMileage,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            var customer = await _context.Customers.FindAsync(dto.CustomerId);
            var result = new VehicleDto
            {
                Id = vehicle.Id,
                CustomerId = vehicle.CustomerId,
                CustomerName = customer?.Name ?? string.Empty,
                RegistrationNumber = vehicle.RegistrationNumber,
                VIN = vehicle.VIN,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                EngineType = vehicle.EngineType,
                CurrentMileage = vehicle.CurrentMileage,
                Color = vehicle.Color,
                CreatedAt = vehicle.CreatedAt
            };

            return CreatedAtAction(nameof(GetVehicle), new { id = vehicle.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating vehicle");
            return StatusCode(500, new { message = "An error occurred while creating the vehicle" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "SuperAdministrator,ShopManager,ServiceAdvisor")]
    public async Task<IActionResult> UpdateVehicle(int id, [FromBody] UpdateVehicleDto dto)
    {
        try
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound(new { message = "Vehicle not found" });
            }

            vehicle.RegistrationNumber = dto.RegistrationNumber;
            vehicle.VIN = dto.VIN;
            vehicle.Make = dto.Make;
            vehicle.Model = dto.Model;
            vehicle.Year = dto.Year;
            vehicle.EngineType = dto.EngineType;
            vehicle.EngineSize = dto.EngineSize;
            vehicle.Transmission = dto.Transmission;
            vehicle.Color = dto.Color;
            vehicle.CurrentMileage = dto.CurrentMileage;
            vehicle.Notes = dto.Notes;
            vehicle.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating vehicle {Id}", id);
            return StatusCode(500, new { message = "An error occurred while updating the vehicle" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdministrator,ShopManager")]
    public async Task<IActionResult> DeleteVehicle(int id)
    {
        try
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound(new { message = "Vehicle not found" });
            }

            // Soft delete
            vehicle.IsDeleted = true;
            vehicle.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting vehicle {Id}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the vehicle" });
        }
    }
}
