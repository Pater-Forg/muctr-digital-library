using Microsoft.AspNetCore.Mvc;

namespace MDLibrary.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
