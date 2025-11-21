using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarServiceShop.Core.DTOs;
using CarServiceShop.Core.Entities;
using CarServiceShop.Infrastructure.Data;
using CarServiceShop.Shared.Enums;

namespace CarServiceShop.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AppointmentsController> _logger;

    public AppointmentsController(ApplicationDbContext context, ILogger<AppointmentsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointments([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        try
        {
            var query = _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Vehicle)
                .Include(a => a.Bay)
                .Include(a => a.Technician)
                .AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(a => a.AppointmentDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(a => a.AppointmentDate <= endDate.Value);
            }

            var appointments = await query.OrderBy(a => a.AppointmentDate).ToListAsync();

            var appointmentDtos = appointments.Select(a => new AppointmentDto
            {
                Id = a.Id,
                AppointmentNumber = a.AppointmentNumber,
                CustomerId = a.CustomerId,
                CustomerName = a.Customer.Name,
                VehicleId = a.VehicleId,
                VehicleDescription = $"{a.Vehicle.Make} {a.Vehicle.Model} {a.Vehicle.Year}",
                AppointmentDate = a.AppointmentDate,
                Duration = a.EstimatedDuration,
                ServiceType = a.ServiceTypeRequested ?? string.Empty,
                BayId = a.BayId,
                BayName = a.Bay?.Name,
                TechnicianId = a.TechnicianId,
                TechnicianName = a.Technician != null ? $"{a.Technician.FirstName} {a.Technician.LastName}" : null,
                Status = a.Status,
                CustomerNotes = a.CustomerNotes,
                InternalNotes = a.InternalNotes,
                ReminderSent = a.ReminderSent,
                CreatedAt = a.CreatedAt
            }).ToList();

            return Ok(appointmentDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appointments");
            return StatusCode(500, new { message = "An error occurred while retrieving appointments" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppointmentDto>> GetAppointment(int id)
    {
        try
        {
            var appointment = await _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Vehicle)
                .Include(a => a.Bay)
                .Include(a => a.Technician)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound(new { message = "Appointment not found" });
            }

            var appointmentDto = new AppointmentDto
            {
                Id = appointment.Id,
                AppointmentNumber = appointment.AppointmentNumber,
                CustomerId = appointment.CustomerId,
                CustomerName = appointment.Customer.Name,
                VehicleId = appointment.VehicleId,
                VehicleDescription = $"{appointment.Vehicle.Make} {appointment.Vehicle.Model} {appointment.Vehicle.Year}",
                AppointmentDate = appointment.AppointmentDate,
                Duration = appointment.EstimatedDuration,
                ServiceType = appointment.ServiceTypeRequested ?? string.Empty,
                BayId = appointment.BayId,
                BayName = appointment.Bay?.Name,
                TechnicianId = appointment.TechnicianId,
                TechnicianName = appointment.Technician != null ? $"{appointment.Technician.FirstName} {appointment.Technician.LastName}" : null,
                Status = appointment.Status,
                CustomerNotes = appointment.CustomerNotes,
                InternalNotes = appointment.InternalNotes,
                ReminderSent = appointment.ReminderSent,
                CreatedAt = appointment.CreatedAt
            };

            return Ok(appointmentDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appointment {Id}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the appointment" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> CreateAppointment([FromBody] CreateAppointmentRequest request)
    {
        try
        {
            // Verify customer exists
            var customerExists = await _context.Customers.AnyAsync(c => c.Id == request.CustomerId);
            if (!customerExists)
            {
                return BadRequest(new { message = "Customer not found" });
            }

            // Verify vehicle exists
            var vehicleExists = await _context.Vehicles.AnyAsync(v => v.Id == request.VehicleId);
            if (!vehicleExists)
            {
                return BadRequest(new { message = "Vehicle not found" });
            }

            // Generate appointment number
            var lastAppointment = await _context.Appointments
                .OrderByDescending(a => a.Id)
                .FirstOrDefaultAsync();
            var appointmentNumber = $"APT-{(lastAppointment?.Id ?? 0) + 1:D6}";

            var appointment = new Appointment
            {
                AppointmentNumber = appointmentNumber,
                CustomerId = request.CustomerId,
                VehicleId = request.VehicleId,
                AppointmentDate = request.AppointmentDate,
                EstimatedDuration = Convert.ToInt32(Math.Round(request.Duration)),
                ServiceTypeRequested = request.ServiceType,
                BayId = request.BayId,
                TechnicianId = request.TechnicianId,
                Status = AppointmentStatus.Scheduled,
                CustomerNotes = request.CustomerNotes,
                InternalNotes = request.InternalNotes,
                ReminderSent = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            // Reload with includes
            var createdAppointment = await _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Vehicle)
                .Include(a => a.Bay)
                .Include(a => a.Technician)
                .FirstAsync(a => a.Id == appointment.Id);

            var appointmentDto = new AppointmentDto
            {
                Id = createdAppointment.Id,
                AppointmentNumber = createdAppointment.AppointmentNumber,
                CustomerId = createdAppointment.CustomerId,
                CustomerName = createdAppointment.Customer.Name,
                VehicleId = createdAppointment.VehicleId,
                VehicleDescription = $"{createdAppointment.Vehicle.Make} {createdAppointment.Vehicle.Model} {createdAppointment.Vehicle.Year}",
                AppointmentDate = createdAppointment.AppointmentDate,
                Duration = createdAppointment.EstimatedDuration,
                ServiceType = createdAppointment.ServiceTypeRequested ?? string.Empty,
                BayId = createdAppointment.BayId,
                BayName = createdAppointment.Bay?.Name,
                TechnicianId = createdAppointment.TechnicianId,
                TechnicianName = createdAppointment.Technician != null ? $"{createdAppointment.Technician.FirstName} {createdAppointment.Technician.LastName}" : null,
                Status = createdAppointment.Status,
                CustomerNotes = createdAppointment.CustomerNotes,
                InternalNotes = createdAppointment.InternalNotes,
                ReminderSent = createdAppointment.ReminderSent,
                CreatedAt = createdAppointment.CreatedAt
            };

            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointmentDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating appointment");
            return StatusCode(500, new { message = "An error occurred while creating the appointment" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAppointment(int id, [FromBody] UpdateAppointmentRequest request)
    {
        try
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound(new { message = "Appointment not found" });
            }

            appointment.AppointmentDate = request.AppointmentDate;
            appointment.EstimatedDuration = Convert.ToInt32(Math.Round(request.Duration));
            appointment.ServiceTypeRequested = request.ServiceType;
            appointment.BayId = request.BayId;
            appointment.TechnicianId = request.TechnicianId;
            appointment.Status = request.Status;
            appointment.CustomerNotes = request.CustomerNotes;
            appointment.InternalNotes = request.InternalNotes;
            appointment.ReminderSent = request.ReminderSent;
            appointment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating appointment {Id}", id);
            return StatusCode(500, new { message = "An error occurred while updating the appointment" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAppointment(int id)
    {
        try
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound(new { message = "Appointment not found" });
            }

            appointment.IsDeleted = true;
            appointment.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting appointment {Id}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the appointment" });
        }
    }
}
