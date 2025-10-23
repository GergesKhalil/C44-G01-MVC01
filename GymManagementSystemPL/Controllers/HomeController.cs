using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystemPL.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
