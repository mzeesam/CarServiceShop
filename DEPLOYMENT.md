# Car Service Shop - Deployment Guide

## Table of Contents
1. [System Requirements](#system-requirements)
2. [Development Setup](#development-setup)
3. [Production Deployment](#production-deployment)
4. [Database Configuration](#database-configuration)
5. [Security Configuration](#security-configuration)
6. [Environment Variables](#environment-variables)
7. [Troubleshooting](#troubleshooting)

## System Requirements

### Development
- .NET 8 SDK or later
- 4GB RAM minimum
- 10GB free disk space
- Code editor (VS Code, Visual Studio 2022, or JetBrains Rider)

### Production
- .NET 8 Runtime
- 8GB RAM recommended
- 50GB free disk space
- Linux, Windows, or macOS server
- Reverse proxy (NGINX, Apache, or IIS)

## Development Setup

### 1. Clone and Build
```bash
git clone https://github.com/mzeesam/CarServiceShop.git
cd CarServiceShop
dotnet restore
dotnet build
```

### 2. Run the API
```bash
cd CarServiceShop.API
dotnet run
```
API will be available at:
- HTTPS: https://localhost:7001
- HTTP: http://localhost:5001
- Swagger: https://localhost:7001/swagger

### 3. Run the Web Application
Open a new terminal:
```bash
cd CarServiceShop.Web
dotnet run
```
Web app will be available at:
- HTTPS: https://localhost:7002
- HTTP: http://localhost:5002

### 4. Default Login Credentials
```
Username: admin
Password: Admin@123
Email: admin@carserviceshop.com
```

## Production Deployment

### Option 1: Linux with NGINX

#### 1. Publish the Applications
```bash
# Publish API
cd CarServiceShop.API
dotnet publish -c Release -o /var/www/carserviceshop/api

# Publish Web
cd ../CarServiceShop.Web
dotnet publish -c Release -o /var/www/carserviceshop/web
```

#### 2. Create systemd Service Files

**API Service** (`/etc/systemd/system/carserviceshop-api.service`):
```ini
[Unit]
Description=Car Service Shop API
After=network.target

[Service]
WorkingDirectory=/var/www/carserviceshop/api
ExecStart=/usr/bin/dotnet /var/www/carserviceshop/api/CarServiceShop.API.dll
Restart=always
RestartSec=10
SyslogIdentifier=carserviceshop-api
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
```

**Web Service** (`/etc/systemd/system/carserviceshop-web.service`):
```ini
[Unit]
Description=Car Service Shop Web
After=network.target

[Service]
WorkingDirectory=/var/www/carserviceshop/web
ExecStart=/usr/bin/dotnet /var/www/carserviceshop/web/CarServiceShop.Web.dll
Restart=always
RestartSec=10
SyslogIdentifier=carserviceshop-web
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
```

#### 3. Start Services
```bash
sudo systemctl enable carserviceshop-api
sudo systemctl start carserviceshop-api
sudo systemctl enable carserviceshop-web
sudo systemctl start carserviceshop-web
```

#### 4. Configure NGINX

Create `/etc/nginx/sites-available/carserviceshop`:
```nginx
# API Server
server {
    listen 80;
    server_name api.carserviceshop.com;

    location / {
        proxy_pass http://localhost:5001;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}

# Web Server
server {
    listen 80;
    server_name www.carserviceshop.com carserviceshop.com;

    location / {
        proxy_pass http://localhost:5002;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

Enable the site:
```bash
sudo ln -s /etc/nginx/sites-available/carserviceshop /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl restart nginx
```

#### 5. Set up SSL with Let's Encrypt
```bash
sudo apt install certbot python3-certbot-nginx
sudo certbot --nginx -d carserviceshop.com -d www.carserviceshop.com
sudo certbot --nginx -d api.carserviceshop.com
```

### Option 2: Windows with IIS

#### 1. Install .NET Hosting Bundle
Download and install from: https://dotnet.microsoft.com/download/dotnet/8.0

#### 2. Publish Applications
```powershell
dotnet publish -c Release -o C:\inetpub\carserviceshop\api
dotnet publish -c Release -o C:\inetpub\carserviceshop\web
```

#### 3. Create IIS Sites
1. Open IIS Manager
2. Create Application Pool: "CarServiceShopAPI"
   - .NET CLR Version: No Managed Code
3. Create Application Pool: "CarServiceShopWeb"
   - .NET CLR Version: No Managed Code
4. Create Website for API:
   - Name: CarServiceShop API
   - Application Pool: CarServiceShopAPI
   - Physical Path: C:\inetpub\carserviceshop\api
   - Binding: Port 5001
5. Create Website for Web:
   - Name: CarServiceShop Web
   - Application Pool: CarServiceShopWeb
   - Physical Path: C:\inetpub\carserviceshop\web
   - Binding: Port 5002

### Option 3: Docker Deployment

**Dockerfile for API** (CarServiceShop.API/Dockerfile):
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CarServiceShop.API/CarServiceShop.API.csproj", "CarServiceShop.API/"]
COPY ["CarServiceShop.Core/CarServiceShop.Core.csproj", "CarServiceShop.Core/"]
COPY ["CarServiceShop.Infrastructure/CarServiceShop.Infrastructure.csproj", "CarServiceShop.Infrastructure/"]
COPY ["CarServiceShop.Shared/CarServiceShop.Shared.csproj", "CarServiceShop.Shared/"]
RUN dotnet restore "CarServiceShop.API/CarServiceShop.API.csproj"
COPY . .
WORKDIR "/src/CarServiceShop.API"
RUN dotnet build "CarServiceShop.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarServiceShop.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarServiceShop.API.dll"]
```

**docker-compose.yml**:
```yaml
version: '3.8'

services:
  api:
    build:
      context: .
      dockerfile: CarServiceShop.API/Dockerfile
    ports:
      - "5001:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Data Source=/app/data/carserviceshop.db
    volumes:
      - ./data:/app/data
    restart: always

  web:
    build:
      context: .
      dockerfile: CarServiceShop.Web/Dockerfile
    ports:
      - "5002:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ApiSettings__BaseUrl=http://api:80/
    depends_on:
      - api
    restart: always
```

Deploy with Docker:
```bash
docker-compose up -d
```

## Database Configuration

### Development (SQLite)
The default configuration uses SQLite, which is perfect for development and small deployments.

**appsettings.json**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=carserviceshop.db"
  }
}
```

### Production (SQL Server)

#### 1. Install SQL Server package
```bash
cd CarServiceShop.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.11
```

#### 2. Update connection string in appsettings.Production.json:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CarServiceShop;User Id=sa;Password=YourStrong@Password;TrustServerCertificate=True"
  }
}
```

#### 3. Update Program.cs to use SQL Server:
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

#### 4. Create migrations for SQL Server:
```bash
dotnet ef migrations add InitialCreate --project CarServiceShop.Infrastructure --startup-project CarServiceShop.API
dotnet ef database update --project CarServiceShop.Infrastructure --startup-project CarServiceShop.API
```

### Production (PostgreSQL)

#### 1. Install PostgreSQL package
```bash
cd CarServiceShop.Infrastructure
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.11
```

#### 2. Update connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=carserviceshop;Username=postgres;Password=yourpassword"
  }
}
```

#### 3. Update Program.cs:
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
```

## Security Configuration

### 1. Change JWT Secret Key
Update `appsettings.Production.json`:
```json
{
  "JwtSettings": {
    "SecretKey": "GENERATE_A_STRONG_RANDOM_KEY_AT_LEAST_32_CHARACTERS_LONG",
    "Issuer": "CarServiceShopAPI",
    "Audience": "CarServiceShopClient",
    "ExpirationMinutes": 60
  }
}
```

Generate a secure key:
```bash
openssl rand -base64 32
```

### 2. Configure CORS for Production
Update Program.cs:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("Production", policy =>
    {
        policy.WithOrigins("https://www.carserviceshop.com", "https://carserviceshop.com")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// In Configure section
app.UseCors("Production");
```

### 3. Enable HTTPS Enforcement
The application automatically enforces HTTPS. Ensure SSL certificates are properly configured.

### 4. Rate Limiting
Install package:
```bash
dotnet add package AspNetCoreRateLimit
```

Configure in Program.cs:
```csharp
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
```

## Environment Variables

### API Configuration
```bash
export ASPNETCORE_ENVIRONMENT=Production
export ConnectionStrings__DefaultConnection="your_connection_string"
export JwtSettings__SecretKey="your_secret_key"
export JwtSettings__Issuer="CarServiceShopAPI"
export JwtSettings__Audience="CarServiceShopClient"
export JwtSettings__ExpirationMinutes=60
```

### Web Configuration
```bash
export ASPNETCORE_ENVIRONMENT=Production
export ApiSettings__BaseUrl="https://api.carserviceshop.com/"
```

## Troubleshooting

### Issue: Application won't start

**Solution:**
```bash
# Check logs
sudo journalctl -u carserviceshop-api -n 50
sudo journalctl -u carserviceshop-web -n 50

# Check if port is in use
sudo netstat -tlnp | grep :5001
sudo netstat -tlnp | grep :5002
```

### Issue: Database connection fails

**Solution:**
1. Verify connection string
2. Check database server is running
3. Verify firewall rules
4. Check user permissions

### Issue: JWT authentication fails

**Solution:**
1. Verify JWT secret key is same across all servers
2. Check token expiration
3. Verify issuer and audience settings

### Issue: 502 Bad Gateway with NGINX

**Solution:**
```bash
# Check if .NET applications are running
sudo systemctl status carserviceshop-api
sudo systemctl status carserviceshop-web

# Check NGINX error logs
sudo tail -f /var/log/nginx/error.log
```

## Monitoring

### Application Logs
Logs are written to:
- Linux: `/var/log/carserviceshop/`
- Windows: `C:\Logs\CarServiceShop\`

### Health Checks
Add health check endpoints:
- API: `https://api.carserviceshop.com/health`
- Web: `https://www.carserviceshop.com/health`

## Backup Strategy

### Database Backup (SQLite)
```bash
# Daily backup
sqlite3 carserviceshop.db ".backup '/backups/carserviceshop-$(date +%Y%m%d).db'"
```

### Database Backup (SQL Server)
```sql
BACKUP DATABASE CarServiceShop 
TO DISK = 'C:\Backups\CarServiceShop.bak' 
WITH FORMAT, COMPRESSION;
```

### Database Backup (PostgreSQL)
```bash
pg_dump -U postgres carserviceshop > carserviceshop-$(date +%Y%m%d).sql
```

## Performance Tuning

### 1. Enable Response Compression
```csharp
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});
```

### 2. Add Response Caching
```csharp
builder.Services.AddResponseCaching();
app.UseResponseCaching();
```

### 3. Database Indexing
Ensure proper indexes are created for frequently queried columns.

### 4. Connection Pooling
Already configured in Entity Framework Core by default.

---

**For additional support, please refer to the main README.md or create an issue on GitHub.**
