using HRMS.Application.Interfaces;
using HRMS.Application.NewFolder;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.WebUI.Controllers
{
    [Route("api/[controller]")]
    public class CompanyController : BaseController
    {
        private readonly ICompanyFacade _companyFacade;
        public CompanyController(ICompanyFacade companyFacade)
        {
            _companyFacade = companyFacade;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var companies = await _companyFacade.GetAllAsync();
            return CreateResponse(200, "Success", companies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var company = await _companyFacade.GetByIdAsync(id);
            if (company == null)
                return CreateResponse(404, "Company not found");
            return CreateResponse(200, "Success", company);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CompanyCreateDto dto)
        {
            if (!ModelState.IsValid)
                return CreateResponse(400, "Validation failed", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            var company = await _companyFacade.CreateAsync(dto);
            return CreateResponse(201, "Company created", company);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CompanyUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return CreateResponse(400, "Validation failed", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            var company = await _companyFacade.UpdateAsync(dto);
            if (company == null)
                return CreateResponse(404, "Company not found");
            return CreateResponse(200, "Company updated", company);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _companyFacade.DeleteAsync(id);
            if (!result)
                return CreateResponse(404, "Company not found");
            return CreateResponse(200, "Company deleted");
        }
    }
} 