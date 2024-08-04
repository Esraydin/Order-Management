
# Company Order Management System

## Project Description

The Company Order Management System is designed to facilitate the management of company orders through a robust backend API. This API will be the core of the application, allowing future expansions such as a Blazor-based admin panel. The project is built with scalability and extensibility in mind, starting with Phase 1.

### Key Features:

- **API Focused**: The frontend communicates with the backend through the API.
- **Onion Architecture**: Ensures a maintainable and scalable codebase.
- **Repository Pattern & CQRS**: Efficient and clean data handling.
- **Service Layer**: Encapsulates business logic.
- **SOLID Principles**: Promotes high-quality and maintainable code.
- **Dependency Injection**: For managing dependencies.
- **CodeFirst with EntityFramework**: MSSQL database using CodeFirst approach.
- **UnitOfWork Pattern**: Manages data operations.
- **Standardized DTOs & ApiResponse**: For consistent data transfer and responses.
- **Unit Testing**: Ensures code quality with 70% coverage.
- **Exception Handling**: Robust system for managing errors.
- **Caching**: Internal caching for Company and ProductCategory entities.
- **Seed Data**: Initial data setup.
- **JWT Authentication**: Secure user authentication.

## Database Entities

### Company : BaseEntity
- Id
- Name
- Description
- User
- Products
- Orders
- CreatedDate
- LastUpdatedDate

### Product : BaseEntity
- Id
- Name
- Description
- StockCount
- Price
- Company
- ProductCategory
- CreatedDate
- LastUpdatedDate

### ProductCategory : BaseEntity
- Id
- Name
- Description
- Products
- CreatedDate
- LastUpdatedDate

### Order : BaseEntity
- Id
- Name
- OrderCount
- UnitPrice
- TotalPrice
- OrderStatus (Pending, Successful, Failed)
- Company
- Product
- User
- CreatedDate
- LastUpdatedDate

### User : BaseEntity
- Id
- Name
- Description
- Orders
- CreatedDate
- LastUpdatedDate

Standard operations related to these entities will be implemented (e.g., creating products, placing orders, user registration). Additional fields can be added as needed.

## Getting Started

Follow these steps to get started with the project:

1. **Clone the Repository**:
   ```sh
   git clone <repository-url>
   cd <repository-directory>
   
2. **Install Dependencies**:
   ```sh
   dotnet restore
  
3. **Create the Database**:
   ```sh
   dotnet ef database update
  
4. **Run the Application**:
   ```sh
   dotnet run
