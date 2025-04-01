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
            // Vérifier si l'utilisateur est connecté
            if (!HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Connexion", "GestionDuCompte");
            }
            return View();
        }

        public IActionResult Paiement()
        {
            // Vérifier si l'utilisateur est connecté
            if (!HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Connexion", "GestionDuCompte");
            }
            return View();
        }

        public IActionResult Validation()
        {
            if (!HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Connexion", "GestionDuCompte");
            }
            return View();
        }

        public IActionResult Factures()
        {
            if (!HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Connexion", "GestionDuCompte");
            }
            return View();
        }
    }
}
