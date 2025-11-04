using HRMS.Application.DTOs.JobTitles;
using HRMS.Application.Interfaces.JobTitles;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTitleController : BaseController<JobTitleController>
    {
        private readonly IJobTitleService _jobTitleService;
        public JobTitleController(IJobTitleService jobTitleService, ILogger<JobTitleController> logger)
            : base(logger)
        {
            _jobTitleService = jobTitleService;
        }

        /// <summary>
        /// Get all job titles
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            Logger.LogInformation("Getting all job titles");
            var jobTitles = await _jobTitleService.GetAllJobTitlesAsync();
            return CreateResponse(200, "Success", jobTitles);
        }

        /// <summary>
        /// Get job title by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return CreateResponse(400, "Invalid job title ID");
            var jobTitle = await _jobTitleService.GetJobTitleByIdAsync(id);
            if (jobTitle == null)
                return CreateResponse(404, "Job title not found");
            return CreateResponse(200, "Success", jobTitle);
        }

        /// <summary>
        /// Create a new job title
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] JobTitleCreateDto dto)
        {
            if (dto == null)
                return CreateResponse(400, "Job title data is required");
            if (!ModelState.IsValid)
                return CreateResponse(400, "Validation failed", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            var jobTitle = await _jobTitleService.CreateJobTitleAsync(dto);
            return CreateResponse(201, "Job title created successfully", jobTitle);
        }

        /// <summary>
        /// Update an existing job title
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] JobTitleUpdateDto dto)
        {
            if (dto == null)
                return CreateResponse(400, "Job title data is required");
            if (!ModelState.IsValid)
                return CreateResponse(400, "Validation failed", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            var jobTitle = await _jobTitleService.UpdateJobTitleAsync(dto);
            if (jobTitle == null)
                return CreateResponse(404, "Job title not found");
            return CreateResponse(200, "Job title updated successfully", jobTitle);
        }

        /// <summary>
        /// Delete a job title by ID
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return CreateResponse(400, "Invalid job title ID");
            await _jobTitleService.DeleteJobTitleAsync(id);
            return CreateResponse(200, "Job title deleted successfully");
        }
    }
} 