using Microsoft.AspNetCore.Mvc;
using BoutiqueEnLigne.Models;

namespace BoutiqueEnLigne.Controllers
{
    public class GestionDuCompteController : Controller
    {
        // Utilisateur par défaut pour les tests
        private static readonly User DefaultUser = new User
        {
            Id = 1,
            Nom = "User",
            Email = "user@test.com",
            MotDePasse = "password123",
            Photo = "~/images/membre1.jpg"
        };

        // Variable statique pour simuler l'état de connexion
        public static bool isLoggedIn = false;

        public IActionResult Index()
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Connexion");
            }
            return View();
        }

        public IActionResult Connexion()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Connexion(string email, string motDePasse)
        {
            if (email == DefaultUser.Email && motDePasse == DefaultUser.MotDePasse)
            {
                isLoggedIn = true;
                return RedirectToAction("Infospersonnelles");
            }
            
            ViewBag.Error = "Email ou mot de passe incorrect";
            return View();
        }

        public IActionResult Inscription()
        {
            return View();
        }

        public IActionResult Infospersonnelles()
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Connexion");
            }
            return View(DefaultUser);
        }

        public IActionResult Commandes()
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Connexion");
            }
            return View();
        }

        public IActionResult Adresses()
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Connexion");
            }
            return View();
        }

        public IActionResult Securite()
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Connexion");
            }
            return View();
        }

        public IActionResult Deconnexion()
        {
            isLoggedIn = false;
            return RedirectToAction("Index", "Accueil");
        }
    }
}
