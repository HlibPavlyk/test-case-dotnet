# Transaction API

## Description
Transaction API is a web application designed to manage and process transactions. It provides a set of RESTful APIs for transaction operations and integrates with various data services.

## Technologies Used
- **.NET 8.0**: The core framework for building the application.
- **Entity Framework Core**: For database migration.
- **Dapper**: For executing SQL queries.
- **SQL Server**: The database used for storing transaction data.
- **Docker**: For containerizing the application.
- **MediatR**: For implementing the mediator pattern.
- **Swagger**: For API documentation.

## Prerequisites
- **Docker**: Ensure Docker is installed and running on your machine.
- **.NET SDK 8.0**: Ensure the .NET SDK is installed.

## Running the Application

### Using Docker Compose
1. **Clone the repository**:
    ```sh
    git clone https://github.com/HlibPavlyk/test-case-dotnet.git
    cd test-case-dotnet
    ```

2. **Setup certificates**:
    ```sh
    dotnet dev-certs https -ep "C:\Users\<username>\.aspnet\https\cert.pfx" -p <password>
    dotnet dev-certs https --trust
    ```
   Replace `<username>` with your Windows username and `<password>` with your desired password.


3. **Update `docker-compose.yml`**:
    ```yaml
        environment:
          - ASPNETCORE_Kestrel__Certificates__Default__Password=<password>
          - ASPNETCORE_Kestrel__Certificates__Default__Path=/https/cert.pfx
    ```
   
   Replace `<password>` with the password you used for the certificate.


4. **Build and run the containers**:
    ```sh
    docker-compose up
    ```

5. **Access the application**:
   - Swagger: `https://localhost:8081`

## License
This project is licensed under the MIT License.