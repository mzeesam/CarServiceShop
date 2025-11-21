using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CarServiceShop.Core.Entities;

namespace CarServiceShop.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Estimate> Estimates { get; set; }
    public DbSet<EstimateItem> EstimateItems { get; set; }
    public DbSet<WorkOrder> WorkOrders { get; set; }
    public DbSet<WorkOrderItem> WorkOrderItems { get; set; }
    public DbSet<Part> Parts { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Bay> Bays { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Customer
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.CustomerNumber).IsUnique();
            entity.HasIndex(e => e.Email);
            entity.Property(e => e.CustomerNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Phone).HasMaxLength(50).IsRequired();
            entity.Property(e => e.CreditLimit).HasPrecision(18, 2);
            
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Configure Vehicle
        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.RegistrationNumber).IsUnique();
            entity.HasIndex(e => e.VIN);
            entity.Property(e => e.RegistrationNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Make).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Model).HasMaxLength(100).IsRequired();
            
            entity.HasOne(e => e.Customer)
                .WithMany(c => c.Vehicles)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Configure Appointment
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.AppointmentNumber).IsUnique();
            entity.Property(e => e.AppointmentNumber).HasMaxLength(50).IsRequired();
            
            entity.HasOne(e => e.Customer)
                .WithMany(c => c.Appointments)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.Vehicle)
                .WithMany(v => v.Appointments)
                .HasForeignKey(e => e.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.Bay)
                .WithMany(b => b.Appointments)
                .HasForeignKey(e => e.BayId)
                .OnDelete(DeleteBehavior.SetNull);
                
            entity.HasOne(e => e.Technician)
                .WithMany(t => t.Appointments)
                .HasForeignKey(e => e.TechnicianId)
                .OnDelete(DeleteBehavior.SetNull);
                
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Configure Estimate
        modelBuilder.Entity<Estimate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.EstimateNumber).IsUnique();
            entity.Property(e => e.EstimateNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.SubTotal).HasPrecision(18, 2);
            entity.Property(e => e.Discount).HasPrecision(18, 2);
            entity.Property(e => e.Tax).HasPrecision(18, 2);
            entity.Property(e => e.Total).HasPrecision(18, 2);
            
            entity.HasOne(e => e.Customer)
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.Vehicle)
                .WithMany()
                .HasForeignKey(e => e.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Configure EstimateItem
        modelBuilder.Entity<EstimateItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Quantity).HasPrecision(18, 2);
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
            entity.Property(e => e.Total).HasPrecision(18, 2);
            
            entity.HasOne(e => e.Estimate)
                .WithMany(est => est.EstimateItems)
                .HasForeignKey(e => e.EstimateId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Configure WorkOrder
        modelBuilder.Entity<WorkOrder>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.WorkOrderNumber).IsUnique();
            entity.Property(e => e.WorkOrderNumber).HasMaxLength(50).IsRequired();
            
            entity.HasOne(e => e.Estimate)
                .WithOne(est => est.WorkOrder)
                .HasForeignKey<WorkOrder>(e => e.EstimateId)
                .OnDelete(DeleteBehavior.SetNull);
                
            entity.HasOne(e => e.Customer)
                .WithMany(c => c.WorkOrders)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.Vehicle)
                .WithMany(v => v.WorkOrders)
                .HasForeignKey(e => e.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.Bay)
                .WithMany(b => b.WorkOrders)
                .HasForeignKey(e => e.BayId)
                .OnDelete(DeleteBehavior.SetNull);
                
            entity.HasOne(e => e.Technician)
                .WithMany(t => t.WorkOrders)
                .HasForeignKey(e => e.TechnicianId)
                .OnDelete(DeleteBehavior.SetNull);
                
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Configure WorkOrderItem
        modelBuilder.Entity<WorkOrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Quantity).HasPrecision(18, 2);
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
            entity.Property(e => e.Total).HasPrecision(18, 2);
            entity.Property(e => e.EstimatedHours).HasPrecision(18, 2);
            entity.Property(e => e.ActualHours).HasPrecision(18, 2);
            
            entity.HasOne(e => e.WorkOrder)
                .WithMany(wo => wo.WorkOrderItems)
                .HasForeignKey(e => e.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(e => e.Service)
                .WithMany(s => s.WorkOrderItems)
                .HasForeignKey(e => e.ServiceId)
                .OnDelete(DeleteBehavior.SetNull);
                
            entity.HasOne(e => e.Part)
                .WithMany(p => p.WorkOrderItems)
                .HasForeignKey(e => e.PartId)
                .OnDelete(DeleteBehavior.SetNull);
                
            entity.HasOne(e => e.Technician)
                .WithMany(t => t.WorkOrderItems)
                .HasForeignKey(e => e.TechnicianId)
                .OnDelete(DeleteBehavior.SetNull);
                
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Configure Part
        modelBuilder.Entity<Part>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.PartNumber).IsUnique();
            entity.Property(e => e.PartNumber).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.CostPrice).HasPrecision(18, 2);
            entity.Property(e => e.RetailPrice).HasPrecision(18, 2);
            entity.Property(e => e.WholesalePrice).HasPrecision(18, 2);
            
            entity.HasOne(e => e.Category)
                .WithMany(c => c.Parts)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
                
            entity.HasOne(e => e.Supplier)
                .WithMany(s => s.Parts)
                .HasForeignKey(e => e.SupplierId)
                .OnDelete(DeleteBehavior.SetNull);
                
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Configure Service
        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ServiceCode).IsUnique();
            entity.Property(e => e.ServiceCode).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.StandardHours).HasPrecision(18, 2);
            entity.Property(e => e.LaborRate).HasPrecision(18, 2);
            entity.Property(e => e.FlatRate).HasPrecision(18, 2);
            
            entity.HasOne(e => e.Category)
                .WithMany(c => c.Services)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
                
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Configure Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            
            entity.HasOne(e => e.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(e => e.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Configure Invoice
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.InvoiceNumber).IsUnique();
            entity.Property(e => e.InvoiceNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.SubTotal).HasPrecision(18, 2);
            entity.Property(e => e.Discount).HasPrecision(18, 2);
            entity.Property(e => e.Tax).HasPrecision(18, 2);
            entity.Property(e => e.Total).HasPrecision(18, 2);
            entity.Property(e => e.AmountPaid).HasPrecision(18, 2);
            entity.Property(e => e.Balance).HasPrecision(18, 2);
            
            entity.HasOne(e => e.WorkOrder)
                .WithOne(wo => wo.Invoice)
                .HasForeignKey<Invoice>(e => e.WorkOrderId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.Customer)
                .WithMany(c => c.Invoices)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Configure Payment
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            
            entity.HasOne(e => e.Invoice)
                .WithMany(i => i.Payments)
                .HasForeignKey(e => e.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Configure Bay
        modelBuilder.Entity<Bay>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.BayNumber).IsUnique();
            entity.Property(e => e.BayNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Configure Supplier
        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.SupplierNumber).IsUnique();
            entity.Property(e => e.SupplierNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.CompanyName).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Phone).HasMaxLength(50).IsRequired();
            entity.Property(e => e.CreditLimit).HasPrecision(18, 2);
            
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Configure User
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
        });
    }
}
