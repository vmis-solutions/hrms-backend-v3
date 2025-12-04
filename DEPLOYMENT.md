# Deployment Guide for Portainer

## Repository Deployment Setup

### Prerequisites
1. Ensure `docker-compose.yml` is committed and pushed to your Git repository
2. Ensure `HRMS.WebUI/Dockerfile` is committed and pushed
3. Verify your repository is accessible from the Portainer server

### Portainer Configuration Steps

1. **Go to Portainer → Stacks → Add stack**

2. **Stack Configuration:**
   - **Name**: `hrms-api` (or your preferred name)
   - **Build method**: Select **"Repository"**

3. **Repository Settings:**
   - **Repository URL**: 
     - Format: `https://github.com/username/repo.git` or
     - Format: `https://gitlab.com/username/repo.git`
     - **Important**: Use HTTPS URL, not SSH
   - **Repository reference**: 
     - Branch name: `main`, `master`, or `issue-15-create-department-crud`
     - **Important**: Make sure this branch exists and has the docker-compose.yml file
   - **Compose path**: `docker-compose.yml`
     - **Important**: Since the file is in the root, use exactly: `docker-compose.yml`
     - Do NOT use: `./docker-compose.yml` or `/docker-compose.yml`

4. **Environment Variables:**
   Add these in the "Environment variables" section:
   ```
   DB_SERVER=your-database-server
   DB_PORT=1433
   DB_NAME=HRMS
   DB_USERNAME=sa
   DB_PASSWORD=YourPassword
   JWT_KEY=your-secret-jwt-key-minimum-32-characters
   JWT_ISSUER=HRMS-API
   JWT_AUDIENCE=HRMS-Client
   JWT_EXPIRY_IN_MINUTES=60
   ENABLE_SWAGGER=true
   ```
   **Note**: `ENABLE_SWAGGER` defaults to `true` if not specified. Set to `false` to disable Swagger in production.

5. **Auto-update** (Optional):
   - Enable if you want automatic redeployment on git push

6. **Click "Deploy the stack"**

## Troubleshooting

### Error: "failed to load the compose file: open /data/compose/XX/docker-compose.yml: no such file or directory"

**Possible Causes and Solutions:**

1. **Compose path is incorrect**
   - ✅ Correct: `docker-compose.yml`
   - ❌ Wrong: `./docker-compose.yml`
   - ❌ Wrong: `/docker-compose.yml`
   - ❌ Wrong: `HRMS.WebUI/docker-compose.yml`

2. **File not in repository**
   - Verify the file is committed: Check your Git repository in a browser
   - Ensure you're on the correct branch
   - Make sure the file is in the root directory, not in a subdirectory

3. **Repository access issues**
   - If private repo: Configure Git credentials in Portainer → Settings → Git credentials
   - Verify the repository URL is correct and accessible
   - Try using HTTPS URL instead of SSH

4. **Branch name incorrect**
   - Verify the branch exists in your repository
   - Check the exact branch name (case-sensitive)

5. **File encoding issues**
   - Ensure the file uses Unix line endings (LF) not Windows (CRLF)
   - The file should be UTF-8 encoded

### Verification Steps

Before deploying, verify:

1. **Check repository structure:**
   ```
   Your-Repo/
   ├── docker-compose.yml          ← Must be here
   ├── HRMS.WebUI/
   │   └── Dockerfile              ← Must be here
   └── ... (other files)
   ```

2. **Verify file exists in Git:**
   - Go to your repository in a web browser
   - Navigate to the root directory
   - Confirm `docker-compose.yml` is visible
   - Click on it to verify the content

3. **Test locally:**
   ```bash
   # Clone your repository to a test location
   git clone <your-repo-url> test-deploy
   cd test-deploy
   # Verify file exists
   ls -la docker-compose.yml
   # Test compose file
   docker-compose config
   ```

## Alternative: Manual File Upload

If repository deployment continues to fail, you can use the "Web editor" method:

1. In Portainer, select **"Web editor"** instead of "Repository"
2. Copy the entire content of your `docker-compose.yml` file
3. Paste it into the web editor
4. Add environment variables
5. Deploy

## Post-Deployment

After successful deployment:

1. **Check container status:**
   - Go to Portainer → Containers
   - Verify `hrms-management-api` is running

2. **Check logs:**
   - Click on the container → Logs
   - Look for any startup errors

3. **Access the API:**
   - URL: `http://your-server-ip:9001`
   - Swagger: `http://your-server-ip:9001/swagger`

4. **Health check:**
   - The container should show as "healthy" after ~40 seconds
   - Health check endpoint: `/swagger/index.html`

## Support

If issues persist:
1. Check Portainer logs: Portainer → Settings → Logs
2. Verify Docker is running on the server
3. Check network connectivity from Portainer to your Git repository
4. Verify repository permissions and access

