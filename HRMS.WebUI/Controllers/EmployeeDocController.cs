using HRMS.Application.DTOs.EmployeeDocs;
using HRMS.Application.Interfaces.EmployeeDocs;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDocController : BaseController<EmployeeDocController>
    {
        private readonly IEmployeeDocService _employeeDocService;
        public EmployeeDocController(IEmployeeDocService employeeDocService, ILogger<EmployeeDocController> logger)
            : base(logger)
        {
            _employeeDocService = employeeDocService;
        }

        /// <summary>
        /// Get all employee documents
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            Logger.LogInformation("Getting all employee documents");
            var employeeDocs = await _employeeDocService.GetAllEmployeeDocsAsync();
            return CreateResponse(200, "Success", employeeDocs);
        }

        /// <summary>
        /// Get employee document by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return CreateResponse(400, "Invalid employee document ID");
            var employeeDoc = await _employeeDocService.GetEmployeeDocByIdAsync(id);
            if (employeeDoc == null)
                return CreateResponse(404, "Employee document not found");
            return CreateResponse(200, "Success", employeeDoc);
        }

        /// <summary>
        /// Get employee documents by employee ID
        /// </summary>
        [HttpGet("employee/{employeeId:guid}")]
        public async Task<IActionResult> GetByEmployeeId(Guid employeeId)
        {
            if (employeeId == Guid.Empty)
                return CreateResponse(400, "Invalid employee ID");
            var employeeDocs = await _employeeDocService.GetEmployeeDocsByEmployeeIdAsync(employeeId);
            return CreateResponse(200, "Success", employeeDocs);
        }

        /// <summary>
        /// Create a new employee document
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] EmployeeDocCreateDto dto)
        {
            if (dto == null)
                return CreateResponse(400, "Employee document data is required");
            if (dto.Document == null || dto.Document.Length == 0)
                return CreateResponse(400, "Document file is required");
            if (!ModelState.IsValid)
                return CreateResponse(400, "Validation failed", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            var employeeDoc = await _employeeDocService.CreateEmployeeDocAsync(dto);
            return CreateResponse(201, "Employee document created successfully", employeeDoc);
        }

        /// <summary>
        /// Update an existing employee document
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] EmployeeDocUpdateDto dto)
        {
            if (dto == null)
                return CreateResponse(400, "Employee document data is required");
            if (!ModelState.IsValid)
                return CreateResponse(400, "Validation failed", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            var employeeDoc = await _employeeDocService.UpdateEmployeeDocAsync(dto);
            if (employeeDoc == null)
                return CreateResponse(404, "Employee document not found");
            return CreateResponse(200, "Employee document updated successfully", employeeDoc);
        }

        /// <summary>
        /// Delete an employee document by ID
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return CreateResponse(400, "Invalid employee document ID");
            await _employeeDocService.DeleteEmployeeDocAsync(id);
            return CreateResponse(200, "Employee document deleted successfully");
        }
    }
} 