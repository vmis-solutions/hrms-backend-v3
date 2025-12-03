using HRMS.Application.Interfaces;
using HRMS.Application.DTOs.Companies;
using Microsoft.AspNetCore.Mvc;
using HRMS.Application.Interfaces.Companies;

namespace HRMS.WebUI.Controllers
{
    [Route("api/[controller]")]
    public class CompanyController : BaseController<CompanyController>
    {
        private readonly ICompanyService _companyService;
        public CompanyController(ICompanyService companyService, ILogger<CompanyController> logger)
            : base(logger)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            Logger.LogInformation("Getting all companies");
            var companies = await _companyService.GetAllAsync();
            return CreateResponse(200, "Success", companies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var company = await _companyService.GetByIdAsync(id);
            if (company == null)
                return CreateResponse(404, "Company not found");
            return CreateResponse(200, "Success", company);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CompanyCreateDto dto)
        {
            if (!ModelState.IsValid)
                return CreateResponse(400, "Validation failed", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            
            try
            {
                var company = await _companyService.CreateAsync(dto);
                return CreateResponse(201, "Company created", company);
            }
            catch (InvalidOperationException ex)
            {
                return CreateResponse(409, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CompanyUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return CreateResponse(400, "Validation failed", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            
            try
            {
                var company = await _companyService.UpdateAsync(dto);
                if (company == null)
                    return CreateResponse(404, "Company not found");
                return CreateResponse(200, "Company updated", company);
            }
            catch (InvalidOperationException ex)
            {
                return CreateResponse(409, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _companyService.DeleteAsync(id);
            if (!result)
                return CreateResponse(404, "Company not found");
            return CreateResponse(200, "Company deleted");
        }
    }
} 