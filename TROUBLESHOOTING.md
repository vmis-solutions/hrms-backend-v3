# Troubleshooting Guide

## Container Shows as "Unhealthy"

### Common Causes and Solutions

#### 1. **Application Not Starting**
Check container logs:
```bash
docker logs hrms-management-api
# Or in Portainer: Containers → hrms-management-api → Logs
```

**Common issues:**
- Database connection failure
- Missing environment variables
- Application crash on startup

#### 2. **Health Check Failing**
The health check verifies that Swagger is accessible. If it fails:

**Check if application is running:**
```bash
# Inside the container
docker exec -it hrms-management-api curl http://localhost:5000/swagger/v1/swagger.json
```

**Verify health check endpoint:**
- Health check uses: `http://localhost:5000/swagger/v1/swagger.json`
- Swagger UI is at root: `http://your-server:9001/`
- Swagger JSON: `http://your-server:9001/swagger/v1/swagger.json`

#### 3. **Database Connection Issues**
Verify environment variables are set correctly:
- `DB_SERVER` - Database server IP/hostname
- `DB_PORT` - Usually `1433` for SQL Server
- `DB_NAME` - Database name
- `DB_USERNAME` - Database username
- `DB_PASSWORD` - Database password

**Test database connection:**
- Ensure database server is accessible from the Docker host
- Verify firewall rules allow connection
- Check SQL Server is configured to accept remote connections

#### 4. **Port Access Issues**
Verify port mapping:
- Container port: `5000`
- Host port: `9001`
- Access URL: `http://172.20.10.168:9001`

**Check if port is accessible:**
```bash
# From another machine
curl http://172.20.10.168:9001/swagger/v1/swagger.json
```

#### 5. **HTTPS Redirection Issues**
If you see redirect loops or connection issues, the HTTPS redirection has been disabled for HTTP deployments. This is expected behavior.

#### 6. **Missing Environment Variables**
Ensure all required environment variables are set in Portainer:
- `DB_SERVER`
- `DB_PORT`
- `DB_NAME`
- `DB_USERNAME`
- `DB_PASSWORD`
- `JWT_KEY` (minimum 32 characters)
- `JWT_ISSUER`
- `JWT_AUDIENCE`

## Cannot Access Swagger UI

### Check 1: Container Status
```bash
docker ps
# Should show container as "Up" and "healthy"
```

### Check 2: Application Logs
Look for startup errors:
```bash
docker logs hrms-management-api --tail 100
```

### Check 3: Network Connectivity
- Verify firewall allows port 9001
- Check if port 9001 is already in use: `netstat -an | grep 9001`
- Try accessing from the server itself: `curl http://localhost:9001/`

### Check 4: Swagger Configuration
- Verify `ENABLE_SWAGGER=true` (or not set, defaults to true)
- Check Swagger is enabled in Program.cs

### Check 5: URL Access
- Swagger UI: `http://172.20.10.168:9001/` (root URL, not `/swagger`)
- Swagger JSON: `http://172.20.10.168:9001/swagger/v1/swagger.json`
- API endpoints: `http://172.20.10.168:9001/api/[controller]`

## Quick Diagnostic Commands

### Check Container Health
```bash
docker inspect hrms-management-api | grep -A 10 Health
```

### Check Container Logs
```bash
docker logs hrms-management-api --tail 50 -f
```

### Test Health Check Manually
```bash
docker exec hrms-management-api curl -f http://localhost:5000/swagger/v1/swagger.json
```

### Check Environment Variables
```bash
docker exec hrms-management-api env | grep -E "DB_|JWT_|ASPNETCORE"
```

### Test Port Access
```bash
# From host
curl http://localhost:9001/swagger/v1/swagger.json

# From another machine
curl http://172.20.10.168:9001/swagger/v1/swagger.json
```

## Common Error Messages

### "Connection refused"
- Application not running
- Wrong port mapping
- Firewall blocking access

### "Database connection failed"
- Check database server is accessible
- Verify connection string environment variables
- Check SQL Server allows remote connections

### "JWT Key not found"
- Ensure `JWT_KEY` environment variable is set
- Key must be at least 32 characters

### "Health check failed"
- Application not responding
- Swagger not enabled
- Wrong health check endpoint

## Restart Container

If issues persist, try restarting:
```bash
docker restart hrms-management-api
# Or in Portainer: Containers → hrms-management-api → Restart
```

## Rebuild and Redeploy

If configuration changes were made:
1. Update files in repository
2. In Portainer: Stacks → hrms-api → Editor → Redeploy
3. Or rebuild: Stacks → hrms-api → Editor → Recreate

