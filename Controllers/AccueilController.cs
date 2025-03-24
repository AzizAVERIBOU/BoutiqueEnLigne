using Microsoft.AspNetCore.Mvc;

namespace BoutiqueEnLigne.Controllers
{
    public class AccueilController : Controller
    {
        public IActionResult Accueil()
        {
            return View();
        }
    }
}
