using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Companies;
using HRMS.Application.Interfaces.Departments;
using HRMS.Application.NewFolder;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Application.UseCases.Companies
{
    public class DepartmentFacade : IDepartmentFacade
    {
        private readonly ICreateDepartmentUseCase _create;
        private readonly IUpdateDepartmentUseCase _update;
        private readonly IDeleteDepartmentUseCase _delete;
        private readonly IGetDepartmentByIdUseCase _getById;
        private readonly IGetAllDepartmentUseCase _getAll;


        public DepartmentFacade(
            ICreateDepartmentUseCase create,
            IUpdateDepartmentUseCase update,
            IDeleteDepartmentUseCase delete,
            IGetDepartmentByIdUseCase getById,
            IGetAllDepartmentUseCase getAll
            )
        {
            _create = create;
            _update = update;
            _delete = delete;
            _getAll = getAll;
            _getById = getById;
        }

        public Task<DepartmentDto> CreateAsync(DepartmentCreateDto dto) => _create.ExecuteAsync(dto);
        public Task<DepartmentDto?> UpdateAsync(DepartmentUpdateDto dto) => _update.ExecuteAsync(dto);
        public Task<bool> DeleteAsync(Guid id) => _delete.ExecuteAsync(id);
        public Task<DepartmentDto?> GetByIdAsync(Guid id) => _getById.ExecuteAsync(id);
        public Task<List<DepartmentDto>> GetAllAsync() => _getAll.ExecuteAsync();
    }
} 