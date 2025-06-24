using HRMS.Application.NewFolder;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Companies
{
    public interface IUpdateCompanyUseCase
    {
        Task<CompanyDto?> ExecuteAsync(CompanyUpdateDto dto);
    }
} 