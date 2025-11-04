using HRMS.Application.DTOs.Departments;
using HRMS.Application.Interfaces.Departments;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : BaseController<DepartmentController>
    {
        private readonly IDepartmentService _departmentService;
        
        public DepartmentController(IDepartmentService departmentService, ILogger<DepartmentController> logger)
            : base(logger)
        {
            _departmentService = departmentService;
        }

        /// <summary>
        /// Get all departments
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            Logger.LogInformation("Getting all departments");
            var departments = await _departmentService.GetAllAsync();
            return CreateResponse(200, "Success", departments);
        }

        /// <summary>
        /// Get department by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return CreateResponse(400, "Invalid department ID");
                
            var department = await _departmentService.GetByIdAsync(id);
            if (department == null)
                return CreateResponse(404, "Department not found");
            return CreateResponse(200, "Success", department);
        }

        /// <summary>
        /// Create a new department
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DepartmentCreateDto dto)
        {
            if (dto == null)
                return CreateResponse(400, "Department data is required");

            if (!ModelState.IsValid)
                return CreateResponse(400, "Validation failed", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            
            var department = await _departmentService.CreateAsync(dto);
            return CreateResponse(201, "Department created successfully", department);
        }

        /// <summary>
        /// Update an existing department
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] DepartmentUpdateDto dto)
        {
            if (dto == null)
                return CreateResponse(400, "Department data is required");

            if (!ModelState.IsValid)
                return CreateResponse(400, "Validation failed", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            
            var department = await _departmentService.UpdateAsync(dto);
            if (department == null)
                return CreateResponse(404, "Department not found");
            return CreateResponse(200, "Department updated successfully", department);
        }

        /// <summary>
        /// Delete a department by ID
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return CreateResponse(400, "Invalid department ID");

            var result = await _departmentService.DeleteAsync(id);
            if (!result)
                return CreateResponse(404, "Department not found");
            return CreateResponse(200, "Department deleted successfully");
        }
    }
}
