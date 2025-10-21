using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystemPL.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
