using HRMS.Application.NewFolder;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Companies
{
    public interface ICreateCompanyUseCase
    {
        Task<CompanyDto> ExecuteAsync(CompanyCreateDto dto);
    }
} 