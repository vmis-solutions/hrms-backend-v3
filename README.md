# HRMS Backend v3

Human Resource Management System Backend API built with .NET 9.0

## Project Structure

```
hrms-backend-v3/
├── HRMS.Application/      # Application layer (DTOs, Interfaces)
├── HRMS.Domain/           # Domain layer (Entities, Enums)
├── HRMS.Infrastructure/   # Infrastructure layer (Repositories, Services, Persistence)
├── HRMS.WebUI/            # Web API layer (Controllers, Program.cs)
├── docker-compose.yml     # Docker Compose configuration
└── HRMS.WebUI/Dockerfile  # Docker build file
```

## Prerequisites

- .NET 9.0 SDK
- SQL Server (for database)
- Docker & Docker Compose (for containerized deployment)

## Local Development

1. Update connection string in `HRMS.WebUI/appsettings.Development.json`
2. Run migrations:
   ```bash
   dotnet ef database update --project HRMS.Infrastructure --startup-project HRMS.WebUI
   ```
3. Run the application:
   ```bash
   dotnet run --project HRMS.WebUI
   ```
4. Access Swagger UI at: `http://localhost:5000`

## Docker Deployment

### Using Docker Compose

1. Create a `.env` file in the root directory with the following variables:
   ```env
   DB_SERVER=your-db-server
   DB_PORT=1433
   DB_NAME=HRMS
   DB_USERNAME=sa
   DB_PASSWORD=YourPassword
   JWT_KEY=your-secret-jwt-key-here
   JWT_ISSUER=HRMS-API
   JWT_AUDIENCE=HRMS-Client
   JWT_EXPIRY_IN_MINUTES=60
   ENABLE_SWAGGER=true
   ```

2. Build and run:
   ```bash
   docker-compose up -d
   ```

3. Access the API at: `http://localhost:9001`
4. Swagger UI: `http://localhost:9001/swagger`

### Repository Deployment (Portainer)

1. **Commit and push your code:**
   ```bash
   git add docker-compose.yml HRMS.WebUI/Dockerfile
   git commit -m "Add Docker configuration"
   git push
   ```

2. **In Portainer:**
   - Go to **Stacks** → **Add stack**
   - Select **Repository** build method
   - **Repository URL**: Your Git repository URL
   - **Repository reference**: Branch name (e.g., `main`, `master`)
   - **Compose path**: `docker-compose.yml`
   - **Environment variables**: Add all required variables (DB_SERVER, DB_PORT, etc.)

3. **Deploy the stack**

## API Endpoints

- **Swagger UI**: `/swagger` (root URL)
- **Health Check**: `/swagger/index.html` (used by Docker healthcheck)

## Environment Variables

| Variable | Description | Required |
|----------|-------------|----------|
| `DB_SERVER` | SQL Server hostname/IP | Yes |
| `DB_PORT` | SQL Server port (default: 1433) | Yes |
| `DB_NAME` | Database name | Yes |
| `DB_USERNAME` | Database username | Yes |
| `DB_PASSWORD` | Database password | Yes |
| `JWT_KEY` | JWT signing key (min 32 characters) | Yes |
| `JWT_ISSUER` | JWT issuer | Yes |
| `JWT_AUDIENCE` | JWT audience | Yes |
| `JWT_EXPIRY_IN_MINUTES` | Token expiry in minutes (default: 60) | No |
| `ENABLE_SWAGGER` | Enable Swagger UI in production (default: true) | No |

## Port Configuration

- **Host Port**: 9001
- **Container Port**: 5000
- The API is accessible at `http://your-server:9001`

## Volumes

- `./HRMS.WebUI/wwwroot/EmployeeDocs` → `/app/wwwroot/EmployeeDocs` (Employee documents)
- `./logs` → `/app/logs` (Application logs)

## Health Check

The container includes a health check that verifies the Swagger endpoint is accessible. Check container health status with:
```bash
docker ps
```

## License

[Your License Here]
