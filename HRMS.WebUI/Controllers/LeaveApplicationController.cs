using HRMS.Application.DTOs.LeaveApplications;
using HRMS.Application.Interfaces.LeaveApplications;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveApplicationController : BaseController<LeaveApplicationController>
    {
        private readonly ILeaveApplicationService _leaveApplicationService;
        public LeaveApplicationController(ILeaveApplicationService leaveApplicationService, ILogger<LeaveApplicationController> logger)
            : base(logger)
        {
            _leaveApplicationService = leaveApplicationService;
        }

        /// <summary>
        /// Get all leave applications
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            Logger.LogInformation("Getting all leave applications");
            var leaveApplications = await _leaveApplicationService.GetAllLeaveApplicationsAsync();
            return CreateResponse(200, "Success", leaveApplications);
        }

        /// <summary>
        /// Get leave application by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return CreateResponse(400, "Invalid leave application ID");
            var leaveApplication = await _leaveApplicationService.GetLeaveApplicationByIdAsync(id);
            if (leaveApplication == null)
                return CreateResponse(404, "Leave application not found");
            return CreateResponse(200, "Success", leaveApplication);
        }

        /// <summary>
        /// Get leave applications by employee ID
        /// </summary>
        [HttpGet("employee/{employeeId:guid}")]
        public async Task<IActionResult> GetByEmployeeId(Guid employeeId)
        {
            if (employeeId == Guid.Empty)
                return CreateResponse(400, "Invalid employee ID");
            var leaveApplications = await _leaveApplicationService.GetLeaveApplicationsByEmployeeIdAsync(employeeId);
            return CreateResponse(200, "Success", leaveApplications);
        }

        /// <summary>
        /// Create a new leave application
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LeaveApplicationCreateDto dto)
        {
            if (dto == null)
                return CreateResponse(400, "Leave application data is required");
            if (!ModelState.IsValid)
                return CreateResponse(400, "Validation failed", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            var leaveApplication = await _leaveApplicationService.CreateLeaveApplicationAsync(dto);
            return CreateResponse(201, "Leave application created successfully", leaveApplication);
        }

        /// <summary>
        /// Update an existing leave application
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LeaveApplicationUpdateDto dto)
        {
            if (dto == null)
                return CreateResponse(400, "Leave application data is required");
            if (!ModelState.IsValid)
                return CreateResponse(400, "Validation failed", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            var leaveApplication = await _leaveApplicationService.UpdateLeaveApplicationAsync(dto);
            if (leaveApplication == null)
                return CreateResponse(404, "Leave application not found");
            return CreateResponse(200, "Leave application updated successfully", leaveApplication);
        }

        /// <summary>
        /// Delete a leave application by ID
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return CreateResponse(400, "Invalid leave application ID");
            await _leaveApplicationService.DeleteLeaveApplicationAsync(id);
            return CreateResponse(200, "Leave application deleted successfully");
        }
    }
} 