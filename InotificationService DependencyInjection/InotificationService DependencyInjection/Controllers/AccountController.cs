using Microsoft.AspNetCore.Mvc;

namespace InotificationService_DependencyInjection.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
