using HRMS.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HRMS.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class RoleController : BaseController<RoleController>
    {
        private readonly IUserFacade _userFacade;
        public RoleController(IUserFacade userFacade, ILogger<RoleController> logger) : base(logger)
        {
            _userFacade = userFacade;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddRole([FromQuery] string roleName)
        {
            var result = await _userFacade.CreateRoleAsync(roleName);
            if (!result) return CreateResponse(400, "Role already exists or could not be created");
            return CreateResponse(201, "Role created");
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignRoleToUser([FromQuery] string userId, [FromQuery] string roleName)
        {
            var result = await _userFacade.AssignRoleToUserAsync(userId, roleName);
            if (!result) return CreateResponse(400, "User or role not found, or could not assign role");
            return CreateResponse(200, "Role assigned to user");
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateRole([FromQuery] string oldRoleName, [FromQuery] string newRoleName)
        {
            var result = await _userFacade.UpdateRoleAsync(oldRoleName, newRoleName);
            if (!result) return CreateResponse(400, "Role not found or could not update role");
            return CreateResponse(200, "Role updated");
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _userFacade.GetAllRolesAsync();
            return CreateResponse(200, "Success", roles);
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveRole([FromQuery] string roleName)
        {
            var result = await _userFacade.RemoveRoleAsync(roleName);
            if (!result) return CreateResponse(404, "Role not found or could not be deleted");
            return CreateResponse(200, "Role deleted");
        }
    }
} 