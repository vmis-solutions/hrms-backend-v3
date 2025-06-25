using HRMS.Application.NewFolder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Companies
{
    public interface IGetAllCompanyUseCase
    {
        Task<List<CompanyDto>> ExecuteAsync();
    }
} 