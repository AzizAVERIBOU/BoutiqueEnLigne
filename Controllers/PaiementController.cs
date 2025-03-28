using Microsoft.AspNetCore.Mvc;

namespace BoutiqueEnLigne.Controllers
{
    public class PaiementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Panier()
        {
            return View();
        }

        public IActionResult Paiement()
        {
            return View();
        }
    }
}
