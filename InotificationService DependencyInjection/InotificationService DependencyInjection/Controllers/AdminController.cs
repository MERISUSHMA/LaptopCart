using Microsoft.AspNetCore.Mvc;

namespace InotificationService_DependencyInjection.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
