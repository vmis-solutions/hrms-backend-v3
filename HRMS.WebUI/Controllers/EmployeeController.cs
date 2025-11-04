using HRMS.Application.DTOs.Employees;
using HRMS.Application.Interfaces.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HRMS.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class EmployeeController : BaseController<EmployeeController>
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
            : base(logger)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            Logger.LogInformation("Getting all employees");
            var employees = await _employeeService.GetAllEmployeesAsync();
            return CreateResponse(200, "Success", employees);
        }

        /// <summary>
        /// Get employee by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return CreateResponse(400, "Invalid employee ID");
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
                return CreateResponse(404, "Employee not found");
            return CreateResponse(200, "Success", employee);
        }

        /// <summary>
        /// Create a new employee
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmployeeCreateDto dto)
        {
            if (dto == null)
                return CreateResponse(400, "Employee data is required");
            if (!ModelState.IsValid)
                return CreateResponse(400, "Validation failed", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            var employee = await _employeeService.CreateEmployeeAsync(dto);
            return CreateResponse(201, "Employee created successfully", employee);
        }

        /// <summary>
        /// Update an existing employee
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EmployeeUpdateDto dto)
        {
            if (dto == null)
                return CreateResponse(400, "Employee data is required");
            if (!ModelState.IsValid)
                return CreateResponse(400, "Validation failed", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            var employee = await _employeeService.UpdateEmployeeAsync(dto);
            if (employee == null)
                return CreateResponse(404, "Employee not found");
            return CreateResponse(200, "Employee updated successfully", employee);
        }

        /// <summary>
        /// Delete an employee by ID
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return CreateResponse(400, "Invalid employee ID");
            await _employeeService.DeleteEmployeeAsync(id);
            return CreateResponse(200, "Employee deleted successfully");
        }
    }
} 