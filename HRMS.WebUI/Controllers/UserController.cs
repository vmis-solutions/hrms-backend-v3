using HRMS.Application.DTOs.Employees;
using HRMS.Application.DTOs.Users;
using HRMS.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HRMS.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : BaseController<UserController>
    {
        private readonly IUserFacade _userFacade;
        public UserController(IUserFacade userFacade, ILogger<UserController> logger) : base(logger)
        {
            _userFacade = userFacade;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateDto dto)
        {
            if (dto == null)
                return CreateResponse(400, "User data is required");
            if (!ModelState.IsValid)
                return CreateResponse(400, "Validation failed", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            
            try
            {
                var user = await _userFacade.CreateUserAsync(dto);
                return CreateResponse(201, "User created successfully", user);
            }
            catch (UserValidationException ex)
            {
                return CreateResponse(400, "Validation failed", ex.ValidationErrors);
            }
            catch (System.Exception ex)
            {
                return CreateResponse(400, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            var users = await _userFacade.GetAllUsersPaginatedAsync(pageNumber, pageSize, search);
            return CreateResponse(200, "Success", users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userFacade.GetUserByIdAsync(id);
            if (user == null)
                return CreateResponse(404, "User not found");
            return CreateResponse(200, "Success", user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UserCreateDto dto)
        {
            if (dto == null)
                return CreateResponse(400, "User data is required");
            if (!ModelState.IsValid)
                return CreateResponse(400, "Validation failed", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            
            try
            {
                var user = await _userFacade.UpdateUserAsync(id, dto);
                return CreateResponse(200, "User updated successfully", user);
            }
            catch (UserValidationException ex)
            {
                return CreateResponse(400, "Validation failed", ex.ValidationErrors);
            }
            catch (System.Exception ex)
            {
                return CreateResponse(400, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userFacade.DeleteUserAsync(id);
            if (!result)
                return CreateResponse(404, "User not found or could not be deleted");
            return CreateResponse(200, "User deleted");
        }
    }
} 