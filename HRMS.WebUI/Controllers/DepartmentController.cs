using HRMS.Application.Interfaces.Companies;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.WebUI.Controllers
{
    public class DepartmentController : Controller
    {
        public DepartmentController()
        {
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
