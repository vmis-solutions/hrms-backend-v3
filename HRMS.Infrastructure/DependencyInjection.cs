using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Companies;
using HRMS.Application.UseCases.Companies;
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

namespace HRMS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register your infrastructure services here
            // Example: services.AddScoped<IYourService, YourServiceImplementation>();
            // Register the DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Register Company repository, unit of work, and facade
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
           // services.AddScoped<ICompanyFacade, CompanyFacade>();

            // Register Company use cases and facade
            services.AddScoped<ICreateCompanyUseCase,CreateCompanyUseCase>();
            services.AddScoped<IUpdateCompanyUseCase, UpdateCompanyUseCase>();
            services.AddScoped<IDeleteCompanyUseCase,DeleteCompanyUseCase>();
            services.AddScoped<IGetCompanyByIdUseCase, GetCompanyByIdUseCase>();
            services.AddScoped<IGetAllCompaniesUseCase, GetAllCompaniesUseCase>();
            services.AddScoped<ICompanyFacade,CompanyFacade>();

            // Add other infrastructure services as needed
            return services;
        }
    }
}
