using Microsoft.AspNetCore.Mvc;

namespace InotificationService_DependencyInjection.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
