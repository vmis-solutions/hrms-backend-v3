using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Companies;
using HRMS.Application.Interfaces.Departments;
using HRMS.Application.Interfaces.DepartmentHrManagers;
using HRMS.Infrastructure.Services;
using HRMS.Infrastructure.Persistence.Data;
using HRMS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using HRMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using HRMS.Infrastructure.Identity;

namespace HRMS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register your infrastructure services here
            // Example: services.AddScoped<IYourService, YourServiceImplementation>();
            // Register the DbContext
            // services.AddDbContext<ApplicationDbContext>(options =>
            //     options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Register Company repository, unit of work, and facade
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
           // services.AddScoped<ICompanyFacade, CompanyFacade>();

            // Add JWT Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not found in configuration")))
                    };
                });

            // Add Authorization with Role-based policies
            services.AddAuthorization(options =>
            {
                // Admin policy - highest level access
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("ADMIN"));
                // HR Manager policy - can manage HR operations
                options.AddPolicy("HRManagerOnly", policy => policy.RequireRole("ADMIN", "HR_MANAGER"));
                // HR Supervisor policy - can supervise HR operations
                options.AddPolicy("HRSupervisorOnly", policy => policy.RequireRole("ADMIN", "HR_MANAGER", "HR_SUPERVISOR"));
                // HR Company policy - company-level HR access
                options.AddPolicy("HRCompanyOnly", policy => policy.RequireRole("ADMIN", "HR_MANAGER", "HR_SUPERVISOR", "HR_COMPANY"));
                // Department Head policy - can manage department operations
                options.AddPolicy("DepartmentHeadOnly", policy => policy.RequireRole("ADMIN", "HR_MANAGER", "HR_SUPERVISOR", "HR_COMPANY", "DEPARTMENT_HEAD"));
                // Employee policy - basic employee access
                options.AddPolicy("EmployeeOnly", policy => policy.RequireRole("ADMIN", "HR_MANAGER", "HR_SUPERVISOR", "HR_COMPANY", "DEPARTMENT_HEAD", "EMPLOYEE"));
            });

            // Register Company service
            services.AddScoped<ICompanyService, CompanyService>();

            // Register Department repository and service
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDepartmentService, DepartmentService>();

            // Register DepartmentHrManager service
            services.AddScoped<IDepartmentHrManagerService, DepartmentHrManagerService>();

            // Register Employee repository and service
            services.AddScoped<HRMS.Application.Interfaces.Employees.IEmployeeRepository, HRMS.Infrastructure.Repositories.EmployeeRepository>();
            services.AddScoped<HRMS.Application.Interfaces.Employees.IEmployeeService, HRMS.Infrastructure.Services.EmployeeService>();

            // Register JobTitle repository and service
            services.AddScoped<HRMS.Application.Interfaces.JobTitles.IJobTitleRepository, HRMS.Infrastructure.Repositories.JobTitleRepository>();
            services.AddScoped<HRMS.Application.Interfaces.JobTitles.IJobTitleService, HRMS.Infrastructure.Services.JobTitleService>();

            // Register LeaveApplication repository and service
            services.AddScoped<HRMS.Application.Interfaces.LeaveApplications.ILeaveApplicationRepository, HRMS.Infrastructure.Repositories.LeaveApplicationRepository>();
            services.AddScoped<HRMS.Application.Interfaces.LeaveApplications.ILeaveApplicationService, HRMS.Infrastructure.Services.LeaveApplicationService>();

            // Register LeaveBalance repository and service
            services.AddScoped<HRMS.Application.Interfaces.LeaveBalances.ILeaveBalanceRepository, HRMS.Infrastructure.Repositories.LeaveBalanceRepository>();
            services.AddScoped<HRMS.Application.Interfaces.LeaveBalances.ILeaveBalanceService, HRMS.Infrastructure.Services.LeaveBalanceService>();

            // Register EmployeeDoc repository and service
            services.AddScoped<HRMS.Application.Interfaces.EmployeeDocs.IEmployeeDocRepository, HRMS.Infrastructure.Repositories.EmployeeDocRepository>();
            services.AddScoped<HRMS.Application.Interfaces.EmployeeDocs.IEmployeeDocService, HRMS.Infrastructure.Services.EmployeeDocService>();

            // Register FileService
            services.AddScoped<HRMS.Application.Interfaces.IFileService, HRMS.Infrastructure.Services.FileService>();

            // Register AuthService
            services.AddScoped<HRMS.Application.Interfaces.IAuthService, HRMS.Infrastructure.Services.AuthService>();

            // Register JWT Service
            services.AddScoped<HRMS.Application.Interfaces.IJwtService, HRMS.Infrastructure.Services.JwtService>();

            // Register Dashboard service
            services.AddScoped<HRMS.Application.Interfaces.Dashboard.IDashboardService, HRMS.Infrastructure.Services.DashboardService>();

            // Register UserFacade
            services.AddScoped<IUserFacade, UserFacade>();

            // Add other infrastructure services as needed
            return services;
        }
    }
}
