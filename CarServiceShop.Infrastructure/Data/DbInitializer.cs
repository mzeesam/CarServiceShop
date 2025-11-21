using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CarServiceShop.Core.Entities;
using CarServiceShop.Shared.Enums;

namespace CarServiceShop.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
    {
        // Create roles
        var roles = new[] 
        { 
            "SuperAdministrator", 
            "ShopManager", 
            "ServiceAdvisor", 
            "Technician", 
            "PartsManager", 
            "Cashier",
            "Customer"
        };

        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole<int> { Name = roleName });
            }
        }

        // Create default admin user
        var adminEmail = "admin@carserviceshop.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        
        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Administrator",
                Role = UserRole.SuperAdministrator,
                IsActive = true,
                EmployeeId = "EMP001"
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "SuperAdministrator");
            }
        }

        // Seed initial service categories
        if (!context.Categories.Any())
        {
            var serviceCategories = new[]
            {
                new Category { Name = "Routine Maintenance", CategoryType = "Service", CreatedAt = DateTime.UtcNow },
                new Category { Name = "Repairs", CategoryType = "Service", CreatedAt = DateTime.UtcNow },
                new Category { Name = "Diagnostics", CategoryType = "Service", CreatedAt = DateTime.UtcNow },
                new Category { Name = "Body Work", CategoryType = "Service", CreatedAt = DateTime.UtcNow },
                new Category { Name = "Detailing", CategoryType = "Service", CreatedAt = DateTime.UtcNow }
            };

            context.Categories.AddRange(serviceCategories);
            await context.SaveChangesAsync();
        }

        // Seed initial parts categories
        if (!context.Categories.Any(c => c.CategoryType == "Part"))
        {
            var partCategories = new[]
            {
                new Category { Name = "Engine Parts", CategoryType = "Part", CreatedAt = DateTime.UtcNow },
                new Category { Name = "Brake Parts", CategoryType = "Part", CreatedAt = DateTime.UtcNow },
                new Category { Name = "Suspension Parts", CategoryType = "Part", CreatedAt = DateTime.UtcNow },
                new Category { Name = "Electrical Parts", CategoryType = "Part", CreatedAt = DateTime.UtcNow },
                new Category { Name = "Filters & Fluids", CategoryType = "Part", CreatedAt = DateTime.UtcNow }
            };

            context.Categories.AddRange(partCategories);
            await context.SaveChangesAsync();
        }

        // Seed initial bays
        if (!context.Bays.Any())
        {
            var bays = new[]
            {
                new Bay { BayNumber = "BAY-01", Name = "General Service Bay 1", BayType = "General", Status = "Available", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Bay { BayNumber = "BAY-02", Name = "General Service Bay 2", BayType = "General", Status = "Available", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Bay { BayNumber = "BAY-03", Name = "Diagnostic Bay", BayType = "Diagnostic", Status = "Available", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Bay { BayNumber = "BAY-04", Name = "Alignment Bay", BayType = "Alignment", Status = "Available", IsActive = true, CreatedAt = DateTime.UtcNow },
            };

            context.Bays.AddRange(bays);
            await context.SaveChangesAsync();
        }

        // Seed common services
        if (!context.Services.Any())
        {
            var maintenanceCategory = context.Categories.FirstOrDefault(c => c.Name == "Routine Maintenance");
            
            var services = new[]
            {
                new Service { ServiceCode = "SVC-001", Name = "Oil Change", Description = "Full synthetic oil change with filter", CategoryId = maintenanceCategory?.Id, StandardHours = 0.5m, LaborRate = 50.00m, IsActive = true, CreatedAt = DateTime.UtcNow },
                new Service { ServiceCode = "SVC-002", Name = "Brake Inspection", Description = "Complete brake system inspection", CategoryId = maintenanceCategory?.Id, StandardHours = 1.0m, LaborRate = 75.00m, IsActive = true, CreatedAt = DateTime.UtcNow },
                new Service { ServiceCode = "SVC-003", Name = "Tire Rotation", Description = "Rotate all four tires", CategoryId = maintenanceCategory?.Id, StandardHours = 0.5m, LaborRate = 40.00m, IsActive = true, CreatedAt = DateTime.UtcNow },
                new Service { ServiceCode = "SVC-004", Name = "Wheel Alignment", Description = "Four-wheel alignment", CategoryId = maintenanceCategory?.Id, StandardHours = 1.5m, LaborRate = 100.00m, IsActive = true, CreatedAt = DateTime.UtcNow },
                new Service { ServiceCode = "SVC-005", Name = "Battery Test", Description = "Battery health test and terminal cleaning", CategoryId = maintenanceCategory?.Id, StandardHours = 0.25m, LaborRate = 25.00m, IsActive = true, CreatedAt = DateTime.UtcNow }
            };

            context.Services.AddRange(services);
            await context.SaveChangesAsync();
        }
    }
}
