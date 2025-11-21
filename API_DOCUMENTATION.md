# Car Service Shop - API Documentation

## Base URL
- Development: `https://localhost:7001`
- Production: `https://api.carserviceshop.com`

## Authentication

All API endpoints (except `/api/auth/login` and `/api/auth/register`) require authentication using JWT Bearer tokens.

### Headers
```
Authorization: Bearer {your_jwt_token}
Content-Type: application/json
```

## Authentication Endpoints

### POST /api/auth/login
Authenticate a user and receive a JWT token.

**Request Body:**
```json
{
  "username": "admin",
  "password": "Admin@123"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "admin",
  "email": "admin@carserviceshop.com",
  "role": "SuperAdministrator",
  "expiration": "2024-01-01T12:00:00Z"
}
```

**Error Responses:**
- `401 Unauthorized`: Invalid credentials
- `500 Internal Server Error`: Server error

### POST /api/auth/register
Register a new user account.

**Request Body:**
```json
{
  "username": "johndoe",
  "email": "john@example.com",
  "password": "SecurePass@123",
  "firstName": "John",
  "lastName": "Doe"
}
```

**Response (200 OK):**
```json
{
  "message": "User registered successfully"
}
```

**Error Responses:**
- `400 Bad Request`: Username/email already exists or invalid data
- `500 Internal Server Error`: Server error

## Customer Endpoints

### GET /api/customers
Get all customers.

**Authorization:** Required (All authenticated users)

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "customerNumber": "CUST-000001",
    "customerType": 1,
    "name": "John Doe",
    "companyName": null,
    "email": "john@example.com",
    "phone": "555-0100",
    "secondaryPhone": null,
    "address": "123 Main St",
    "city": "New York",
    "state": "NY",
    "zipCode": "10001",
    "taxNumber": null,
    "creditLimit": 5000.00,
    "isActive": true,
    "notes": "VIP Customer",
    "createdAt": "2024-01-01T10:00:00Z"
  }
]
```

### GET /api/customers/{id}
Get a specific customer by ID.

**Authorization:** Required

**Response (200 OK):**
```json
{
  "id": 1,
  "customerNumber": "CUST-000001",
  "customerType": 1,
  "name": "John Doe",
  ...
}
```

**Error Responses:**
- `404 Not Found`: Customer not found

### POST /api/customers
Create a new customer.

**Authorization:** Required (SuperAdministrator, ShopManager, ServiceAdvisor, Cashier)

**Request Body:**
```json
{
  "customerType": 1,
  "name": "John Doe",
  "companyName": null,
  "email": "john@example.com",
  "phone": "555-0100",
  "secondaryPhone": "555-0101",
  "address": "123 Main St",
  "city": "New York",
  "state": "NY",
  "zipCode": "10001",
  "taxNumber": null,
  "creditLimit": 5000.00,
  "notes": "VIP Customer"
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "customerNumber": "CUST-000001",
  ...
}
```

### PUT /api/customers/{id}
Update an existing customer.

**Authorization:** Required (SuperAdministrator, ShopManager, ServiceAdvisor, Cashier)

**Request Body:**
```json
{
  "customerType": 1,
  "name": "John Doe",
  "email": "john@example.com",
  "phone": "555-0100",
  "isActive": true,
  ...
}
```

**Response (204 No Content)**

**Error Responses:**
- `404 Not Found`: Customer not found

### DELETE /api/customers/{id}
Delete a customer (soft delete).

**Authorization:** Required (SuperAdministrator, ShopManager)

**Response (204 No Content)**

**Error Responses:**
- `404 Not Found`: Customer not found

## Vehicle Endpoints

### GET /api/vehicles
Get all vehicles or filter by customer.

**Authorization:** Required

**Query Parameters:**
- `customerId` (optional): Filter vehicles by customer ID

**Example:** `/api/vehicles?customerId=1`

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "customerId": 1,
    "customerName": "John Doe",
    "registrationNumber": "ABC-123",
    "vin": "1HGBH41JXMN109186",
    "make": "Toyota",
    "model": "Camry",
    "year": 2020,
    "engineType": 1,
    "currentMileage": 45000,
    "color": "Silver",
    "createdAt": "2024-01-01T10:00:00Z"
  }
]
```

### GET /api/vehicles/{id}
Get a specific vehicle by ID.

**Authorization:** Required

**Response (200 OK):**
```json
{
  "id": 1,
  "customerId": 1,
  "customerName": "John Doe",
  ...
}
```

### POST /api/vehicles
Create a new vehicle.

**Authorization:** Required (SuperAdministrator, ShopManager, ServiceAdvisor, Cashier)

**Request Body:**
```json
{
  "customerId": 1,
  "registrationNumber": "ABC-123",
  "vin": "1HGBH41JXMN109186",
  "make": "Toyota",
  "model": "Camry",
  "year": 2020,
  "engineType": 1,
  "engineSize": "2.5L",
  "transmission": "Automatic",
  "color": "Silver",
  "currentMileage": 45000,
  "notes": "Regular customer vehicle"
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "customerId": 1,
  ...
}
```

**Error Responses:**
- `400 Bad Request`: Customer not found or invalid data

### PUT /api/vehicles/{id}
Update an existing vehicle.

**Authorization:** Required (SuperAdministrator, ShopManager, ServiceAdvisor)

**Request Body:**
```json
{
  "registrationNumber": "ABC-123",
  "make": "Toyota",
  "model": "Camry",
  "year": 2020,
  "currentMileage": 50000,
  ...
}
```

**Response (204 No Content)**

### DELETE /api/vehicles/{id}
Delete a vehicle (soft delete).

**Authorization:** Required (SuperAdministrator, ShopManager)

**Response (204 No Content)**

## Enumerations

### CustomerType
```
1 = Individual
2 = Fleet
3 = Corporate
4 = VIP
```

### VehicleEngineType
```
1 = Petrol
2 = Diesel
3 = Hybrid
4 = Electric
5 = Other
```

### AppointmentStatus
```
1 = Scheduled
2 = Confirmed
3 = CheckedIn
4 = InProgress
5 = Completed
6 = NoShow
7 = Cancelled
8 = Rescheduled
```

### WorkOrderStatus
```
1 = Open
2 = Assigned
3 = InProgress
4 = WaitingForParts
5 = WaitingForApproval
6 = QualityCheck
7 = ReadyForPickup
8 = Completed
9 = OnHold
10 = Cancelled
```

### EstimateStatus
```
1 = Draft
2 = Sent
3 = Approved
4 = Rejected
5 = Expired
6 = Converted
```

### PaymentStatus
```
1 = Unpaid
2 = Partial
3 = Paid
4 = Overdue
5 = Refunded
```

### PaymentMethod
```
1 = Cash
2 = Card
3 = BankTransfer
4 = Cheque
5 = MobileMoney
```

### Priority
```
1 = Normal
2 = Urgent
3 = Critical
```

### UserRole
```
1 = SuperAdministrator
2 = ShopManager
3 = ServiceAdvisor
4 = Technician
5 = PartsManager
6 = Cashier
7 = Customer
```

## Error Responses

All endpoints may return the following error responses:

### 400 Bad Request
```json
{
  "message": "Invalid request data",
  "errors": {
    "fieldName": ["Error message"]
  }
}
```

### 401 Unauthorized
```json
{
  "message": "Unauthorized access"
}
```

### 403 Forbidden
```json
{
  "message": "Insufficient permissions"
}
```

### 404 Not Found
```json
{
  "message": "Resource not found"
}
```

### 500 Internal Server Error
```json
{
  "message": "An error occurred while processing your request"
}
```

## Rate Limiting

API requests are limited to prevent abuse:
- 100 requests per minute per IP address
- 1000 requests per hour per IP address

When rate limit is exceeded, you'll receive a `429 Too Many Requests` response.

## Pagination

For endpoints that return large datasets, use pagination parameters:

**Query Parameters:**
- `page`: Page number (default: 1)
- `pageSize`: Items per page (default: 10, max: 100)

**Example:** `/api/customers?page=2&pageSize=20`

**Response Headers:**
```
X-Pagination-TotalCount: 150
X-Pagination-TotalPages: 8
X-Pagination-CurrentPage: 2
X-Pagination-PageSize: 20
```

## API Versioning

Currently, the API is at version 1.0. Future versions will be accessible via:
- `/api/v1/customers`
- `/api/v2/customers`

## Support

For API support:
- GitHub Issues: https://github.com/mzeesam/CarServiceShop/issues
- Email: support@carserviceshop.com

## Testing with Swagger

Access interactive API documentation at:
- Development: https://localhost:7001/swagger
- Production: https://api.carserviceshop.com/swagger

## Testing with cURL

### Login Example
```bash
curl -X POST https://localhost:7001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"Admin@123"}'
```

### Get Customers Example
```bash
TOKEN="your_jwt_token_here"
curl -X GET https://localhost:7001/api/customers \
  -H "Authorization: Bearer $TOKEN"
```

### Create Customer Example
```bash
TOKEN="your_jwt_token_here"
curl -X POST https://localhost:7001/api/customers \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "customerType": 1,
    "name": "Jane Smith",
    "email": "jane@example.com",
    "phone": "555-0200",
    "creditLimit": 3000.00
  }'
```
