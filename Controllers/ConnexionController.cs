using Microsoft.AspNetCore.Mvc;
using BoutiqueEnLigne.Models;
using BoutiqueEnLigne.Models.User;
using BoutiqueEnLigne.Data;

namespace BoutiqueEnLigne.Controllers
{
    public class ConnexionController : Controller
    {
        private readonly BoutiqueEnLigneContext _context;

        public ConnexionController(BoutiqueEnLigneContext context)
        {
            _context = context;
        }

        // GET: /Connexion
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Connexion
        [HttpPost]
        public IActionResult Index(string email, string motDePasse)
        {
            // Pour le moment, on utilise des identifiants en dur
            if (email == "user@test.com" && motDePasse == "password123")
            {
                HttpContext.Session.SetString("UserId", "1");
                HttpContext.Session.SetString("UserEmail", email);
                return RedirectToAction("Index", "Accueil");
            }

            // Si la connexion échoue, on affiche un message d'erreur
            ModelState.AddModelError("", "Email ou mot de passe incorrect");
            return View();
        }

        // GET: /Connexion/Deconnexion
        public IActionResult Deconnexion()
        {
            // Supprimer les informations de session
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Accueil");
        }
    }
} 