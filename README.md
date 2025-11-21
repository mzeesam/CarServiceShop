# Car Service Shop Management System

A comprehensive Car Service & Maintenance Shop Management System built with .NET 8, Blazor Server, and Entity Framework Core.

## 🚀 Features

### Core Functionality
- **Customer Management** - Register and manage individual, fleet, corporate, and VIP customers
- **Vehicle Management** - Track vehicles with complete service history
- **Appointment Scheduling** - Calendar-based appointment system with technician assignment
- **Service Estimates** - Create detailed estimates with approval workflows
- **Work Order Management** - Track jobs from creation to completion
- **Inventory Management** - Parts catalog with stock tracking and alerts
- **Invoicing & Payments** - Generate invoices and process multiple payment methods
- **User Management** - Role-based access control with 7 user roles

### User Roles
1. **Super Administrator** - Full system access
2. **Shop Manager** - Dashboard overview and business management
3. **Service Advisor** - Customer intake and service coordination
4. **Technician/Mechanic** - Work order execution and updates
5. **Parts Manager** - Inventory and procurement management
6. **Cashier/Receptionist** - Payment processing and basic customer service
7. **Customer** - View service history and book appointments

## 🏗️ Architecture

### Technology Stack
- **Frontend**: Blazor Server (.NET 8) with MudBlazor UI components
- **Backend API**: ASP.NET Core Web API (.NET 8)
- **Database**: SQLite with Entity Framework Core
- **Authentication**: ASP.NET Core Identity with JWT tokens
- **ORM**: Entity Framework Core 8

### Project Structure
```
CarServiceShop/
├── CarServiceShop.API/              # REST API project
│   ├── Controllers/                 # API controllers
│   └── Program.cs                   # API configuration
├── CarServiceShop.Core/             # Business logic & entities
│   ├── Entities/                    # Domain models
│   ├── DTOs/                        # Data transfer objects
│   └── Interfaces/                  # Service interfaces
├── CarServiceShop.Infrastructure/   # Data access layer
│   ├── Data/                        # DbContext & migrations
│   └── Repositories/                # Repository implementations
├── CarServiceShop.Shared/           # Shared code
│   ├── Enums/                       # Enumerations
│   └── Constants/                   # Constants
└── CarServiceShop.Web/              # Blazor Server UI
    └── Components/                  # Blazor components
```

## 🛠️ Setup Instructions

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Any code editor (Visual Studio, VS Code, Rider)

### Installation Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/mzeesam/CarServiceShop.git
   cd CarServiceShop
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

4. **Run the API** (Terminal 1)
   ```bash
   cd CarServiceShop.API
   dotnet run
   ```
   The API will start at `https://localhost:7001` and `http://localhost:5001`

5. **Run the Web Application** (Terminal 2)
   ```bash
   cd CarServiceShop.Web
   dotnet run
   ```
   The web app will start at `https://localhost:7002` and `http://localhost:5002`

### Database Setup

The database is automatically created and seeded when you first run the API project. The SQLite database file (`carserviceshop.db`) will be created in the API project directory.

#### Default Admin Credentials
- **Username**: `admin`
- **Password**: `Admin@123`
- **Email**: `admin@carserviceshop.com`

## 📊 Database Schema

### Core Entities
- **Customers** - Customer information and contact details
- **Vehicles** - Vehicle details linked to customers
- **Appointments** - Scheduled service appointments
- **Estimates** - Service estimates and quotations
- **WorkOrders** - Active service jobs
- **WorkOrderItems** - Individual services and parts on work orders
- **Parts** - Inventory parts catalog
- **Services** - Service catalog with pricing
- **Invoices** - Customer invoices
- **Payments** - Payment transactions
- **Bays** - Service bay management
- **Suppliers** - Parts suppliers
- **Users** - System users with roles

## 🔐 Security

- **Authentication**: JWT-based authentication
- **Authorization**: Role-based access control (RBAC)
- **Password Policy**: Minimum 8 characters with uppercase, lowercase, digit, and special character
- **CORS**: Configured for development (adjust for production)
- **HTTPS**: Enforced in production

## 🌐 API Documentation

Once the API is running, access Swagger documentation at:
- `https://localhost:7001/swagger`

### Example API Endpoints

#### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/register` - Register new user

#### Customers
- `GET /api/customers` - Get all customers
- `GET /api/customers/{id}` - Get customer by ID
- `POST /api/customers` - Create new customer
- `PUT /api/customers/{id}` - Update customer
- `DELETE /api/customers/{id}` - Delete customer

## 🧪 Testing

The API includes built-in Swagger UI for testing endpoints interactively.

### Testing with Swagger
1. Start the API project
2. Navigate to `https://localhost:7001/swagger`
3. Click "Authorize" and login to get JWT token
4. Use the token to test protected endpoints

## 📝 Development

### Adding New Features

1. **Create Entity** in `CarServiceShop.Core/Entities`
2. **Add DbSet** in `ApplicationDbContext`
3. **Create DTOs** in `CarServiceShop.Core/DTOs`
4. **Add Repository** in `CarServiceShop.Infrastructure/Repositories`
5. **Create Controller** in `CarServiceShop.API/Controllers`
6. **Build UI Components** in `CarServiceShop.Web/Components`

### Code Style
- Follow standard C# naming conventions
- Use async/await for asynchronous operations
- Implement proper error handling and logging
- Keep controllers thin, business logic in services

## 📦 Dependencies

### Main Packages
- Microsoft.EntityFrameworkCore.Sqlite 8.0.11
- Microsoft.AspNetCore.Identity.EntityFrameworkCore 8.0.11
- Microsoft.AspNetCore.Authentication.JwtBearer 8.0.11
- MudBlazor 8.0.0

## 🚢 Deployment

### Production Considerations
1. Change JWT secret key in `appsettings.json`
2. Use SQL Server or PostgreSQL instead of SQLite
3. Configure proper CORS policies
4. Enable HTTPS enforcement
5. Set up proper logging (e.g., Serilog)
6. Implement rate limiting
7. Add API versioning
8. Set up CI/CD pipelines

## 🤝 Contributing

This is an enterprise application template. Feel free to fork and customize for your specific needs.

## 📄 License

This project is provided as-is for educational and commercial use.

## 📧 Support

For issues or questions, please create an issue in the GitHub repository.

---

**Built with ❤️ using .NET 8 and Blazor**
