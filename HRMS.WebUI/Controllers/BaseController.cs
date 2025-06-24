using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HRMS.WebUI.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly ILogger Logger;

        public BaseController(ILogger logger)
        {
            Logger = logger;
        }

        protected IActionResult CreateResponse<T>(int statusCode, string message, T? data = default)
        {
            var response = new ApiResponse<T>
            {
                Success = statusCode >= 200 && statusCode < 300,
                Message = message,
                Data = data
            };
            return StatusCode(statusCode, response);
        }

        protected IActionResult CreateResponse(int statusCode, string message)
        {
            var response = new ApiResponse<object>
            {
                Success = statusCode >= 200 && statusCode < 300,
                Message = message
            };
            return StatusCode(statusCode, response);
        }
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
    }
} 