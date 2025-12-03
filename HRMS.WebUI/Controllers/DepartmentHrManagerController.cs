using HRMS.Application.DTOs.Departments;
using HRMS.Application.Interfaces.DepartmentHrManagers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HRMS.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentHrManagerController : BaseController<DepartmentHrManagerController>
    {
        private readonly IDepartmentHrManagerService _departmentHrManagerService;
        
        public DepartmentHrManagerController(
            IDepartmentHrManagerService departmentHrManagerService, 
            ILogger<DepartmentHrManagerController> logger)
            : base(logger)
        {
            _departmentHrManagerService = departmentHrManagerService;
        }

        /// <summary>
        /// Get all Department HR Managers by department ID
        /// </summary>
        [HttpGet("department/{departmentId:guid}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetByDepartmentId(Guid departmentId)
        {
            if (departmentId == Guid.Empty)
                return CreateResponse(400, "Invalid department ID");

            Logger.LogInformation("Getting HR managers for department {DepartmentId}", departmentId);
            var hrManagers = await _departmentHrManagerService.GetByDepartmentIdAsync(departmentId);
            return CreateResponse(200, "Success", hrManagers);
        }

        /// <summary>
        /// Delete a Department HR Manager by ID
        /// </summary>
        [HttpDelete("{id:guid}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return CreateResponse(400, "Invalid HR manager ID");

            Logger.LogInformation("Deleting HR manager {Id}", id);
            var result = await _departmentHrManagerService.DeleteAsync(id);
            if (!result)
                return CreateResponse(404, "HR manager not found");
            
            return CreateResponse(200, "HR manager deleted successfully");
        }
    }
}

