using HRMS.Application.DTOs.LeaveBalances;
using HRMS.Application.Interfaces.LeaveBalances;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveBalanceController : BaseController<LeaveBalanceController>
    {
        private readonly ILeaveBalanceService _leaveBalanceService;
        public LeaveBalanceController(ILeaveBalanceService leaveBalanceService, ILogger<LeaveBalanceController> logger)
            : base(logger)
        {
            _leaveBalanceService = leaveBalanceService;
        }

        /// <summary>
        /// Get all leave balances
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            Logger.LogInformation("Getting all leave balances");
            var leaveBalances = await _leaveBalanceService.GetAllLeaveBalancesAsync();
            return CreateResponse(200, "Success", leaveBalances);
        }

        /// <summary>
        /// Get leave balance by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return CreateResponse(400, "Invalid leave balance ID");
            var leaveBalance = await _leaveBalanceService.GetLeaveBalanceByIdAsync(id);
            if (leaveBalance == null)
                return CreateResponse(404, "Leave balance not found");
            return CreateResponse(200, "Success", leaveBalance);
        }

        /// <summary>
        /// Get leave balance by employee ID and year
        /// </summary>
        [HttpGet("employee/{employeeId:guid}/year/{year:int}")]
        public async Task<IActionResult> GetByEmployeeIdAndYear(Guid employeeId, int year)
        {
            if (employeeId == Guid.Empty)
                return CreateResponse(400, "Invalid employee ID");
            if (year <= 0)
                return CreateResponse(400, "Invalid year");
            var leaveBalance = await _leaveBalanceService.GetLeaveBalanceByEmployeeIdAndYearAsync(employeeId, year);
            if (leaveBalance == null)
                return CreateResponse(404, "Leave balance not found");
            return CreateResponse(200, "Success", leaveBalance);
        }

        /// <summary>
        /// Get leave balances by employee ID
        /// </summary>
        [HttpGet("employee/{employeeId:guid}")]
        public async Task<IActionResult> GetByEmployeeId(Guid employeeId)
        {
            if (employeeId == Guid.Empty)
                return CreateResponse(400, "Invalid employee ID");
            var leaveBalances = await _leaveBalanceService.GetLeaveBalancesByEmployeeIdAsync(employeeId);
            return CreateResponse(200, "Success", leaveBalances);
        }

        /// <summary>
        /// Create a new leave balance
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LeaveBalanceCreateDto dto)
        {
            if (dto == null)
                return CreateResponse(400, "Leave balance data is required");
            if (!ModelState.IsValid)
                return CreateResponse(400, "Validation failed", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            var leaveBalance = await _leaveBalanceService.CreateLeaveBalanceAsync(dto);
            return CreateResponse(201, "Leave balance created successfully", leaveBalance);
        }

        /// <summary>
        /// Update an existing leave balance
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LeaveBalanceUpdateDto dto)
        {
            if (dto == null)
                return CreateResponse(400, "Leave balance data is required");
            if (!ModelState.IsValid)
                return CreateResponse(400, "Validation failed", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            var leaveBalance = await _leaveBalanceService.UpdateLeaveBalanceAsync(dto);
            if (leaveBalance == null)
                return CreateResponse(404, "Leave balance not found");
            return CreateResponse(200, "Leave balance updated successfully", leaveBalance);
        }

        /// <summary>
        /// Delete a leave balance by ID
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return CreateResponse(400, "Invalid leave balance ID");
            await _leaveBalanceService.DeleteLeaveBalanceAsync(id);
            return CreateResponse(200, "Leave balance deleted successfully");
        }
    }
} 