using HRMS.Application.DTOs.Departments;
using HRMS.Application.Interfaces.Departments;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        /// <summary>
        /// Get departments managed by the current user (HR Company role) or all if not HR Company
        /// </summary>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetManagedDepartments")]
        public async Task<IActionResult> GetManagedDepartments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            if (string.IsNullOrEmpty(userId))
                return CreateResponse(401, "User identifier not found in token");

            var isHrCompany = User.IsInRole("HR_COMPANY");
            var departments = await _departmentService.GetDepartmentsForCurrentUserAsync(userId, isHrCompany);

            return CreateResponse(200, "Success", departments);
        }

        /// <summary>
        /// Get all HR managers assigned to a department
        /// </summary>
        [HttpGet("{departmentId:guid}/hr-managers")]
        public async Task<IActionResult> GetHrManagers(Guid departmentId)
        {
            if (departmentId == Guid.Empty)
                return CreateResponse(400, "Invalid department ID");

            var hrManagers = await _departmentService.GetHrManagersByDepartmentIdAsync(departmentId);
            return CreateResponse(200, "Success", hrManagers);
        }

        /// <summary>
        /// Assign HR managers to a department
        /// </summary>
        [HttpPost("hr-managers/assign")]
        public async Task<IActionResult> AssignHrManagers([FromBody] AssignHrManagersDto dto)
        {
            if (dto == null)
                return CreateResponse(400, "Assignment data is required");

            if (!ModelState.IsValid)
                return CreateResponse(400, "Validation failed", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());

            if (dto.EmployeeIds == null || dto.EmployeeIds.Count == 0)
                return CreateResponse(400, "At least one employee ID is required");

            var result = await _departmentService.AssignHrManagersAsync(dto);
            if (!result)
                return CreateResponse(400, "Failed to assign HR managers. Please verify department and employee IDs exist.");

            return CreateResponse(200, "HR managers assigned successfully");
        }

        /// <summary>
        /// Remove an HR manager from a department
        /// </summary>
        [HttpDelete("hr-managers/{hrManagerId:guid}")]
        public async Task<IActionResult> RemoveHrManager(Guid hrManagerId)
        {
            if (hrManagerId == Guid.Empty)
                return CreateResponse(400, "Invalid HR manager ID");

            var result = await _departmentService.RemoveHrManagerAsync(hrManagerId);
            if (!result)
                return CreateResponse(404, "HR manager assignment not found");

            return CreateResponse(200, "HR manager removed successfully");
        }
    }
}
