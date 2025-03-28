using Microsoft.AspNetCore.Mvc;

namespace BoutiqueEnLigne.Controllers
{
    public class AutresController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult APropos()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
