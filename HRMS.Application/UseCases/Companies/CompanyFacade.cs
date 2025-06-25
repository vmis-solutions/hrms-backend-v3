using HRMS.Application.Interfaces.Companies;
using HRMS.Application.NewFolder;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Application.UseCases.Companies
{
    public class CompanyFacade : ICompanyFacade
    {
        private readonly ICreateCompanyUseCase _create;
        private readonly IUpdateCompanyUseCase _update;
        private readonly IDeleteCompanyUseCase _delete;
        private readonly IGetCompanyByIdUseCase _getById;
        private readonly IGetAllCompanyUseCase _getAll;

        public CompanyFacade(
            ICreateCompanyUseCase create,
            IUpdateCompanyUseCase update,
            IDeleteCompanyUseCase delete,
            IGetCompanyByIdUseCase getById,
            IGetAllCompanyUseCase getAll)
        {
            _create = create;
            _update = update;
            _delete = delete;
            _getById = getById;
            _getAll = getAll;
        }

        public Task<CompanyDto> CreateAsync(CompanyCreateDto dto) => _create.ExecuteAsync(dto);
        public Task<CompanyDto?> UpdateAsync(CompanyUpdateDto dto) => _update.ExecuteAsync(dto);
        public Task<bool> DeleteAsync(Guid id) => _delete.ExecuteAsync(id);
        public Task<CompanyDto?> GetByIdAsync(Guid id) => _getById.ExecuteAsync(id);
        public Task<List<CompanyDto>> GetAllAsync() => _getAll.ExecuteAsync();
    }
} 