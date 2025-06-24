using HRMS.Infrastructure.Persistence.Data;
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
            services.AddScoped<ICompanyFacade, CompanyFacade>();

            // Register Company use cases and facade
            services.AddScoped<ICreateCompanyUseCase, HRMS.Application.UseCases.Companies.CreateCompanyUseCase>();
            services.AddScoped<IUpdateCompanyUseCase, HRMS.Application.UseCases.Companies.UpdateCompanyUseCase>();
            services.AddScoped<IDeleteCompanyUseCase, HRMS.Application.UseCases.Companies.DeleteCompanyUseCase>();
            services.AddScoped<IGetCompanyByIdUseCase, HRMS.Application.UseCases.Companies.GetCompanyByIdUseCase>();
            services.AddScoped<IGetAllCompaniesUseCase, HRMS.Application.UseCases.Companies.GetAllCompaniesUseCase>();
            services.AddScoped<ICompanyFacade, HRMS.Application.UseCases.Companies.CompanyFacade>();

            // Add other infrastructure services as needed
            return services;
        }
    }
}
