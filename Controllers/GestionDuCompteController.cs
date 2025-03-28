using Microsoft.AspNetCore.Mvc;

namespace BoutiqueEnLigne.Controllers
{
    public class GestionDuCompteController : Controller
    {
        public IActionResult Connexion()
        {
            return View();
        }

        public IActionResult Inscription()
        {
            return View();
        }

        public IActionResult Infospersonnelles()
        {
            return View();
        }

        public IActionResult Commandes()
        {
            return View();
        }

        public IActionResult Adresses()
        {
            return View();
        }

        public IActionResult Securite()
        {
            return View();
        }

        public IActionResult Deconnexion()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
