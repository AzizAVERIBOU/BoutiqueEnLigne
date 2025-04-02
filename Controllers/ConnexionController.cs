using Microsoft.AspNetCore.Mvc;
using BoutiqueEnLigne.Models;
using BoutiqueEnLigne.Models.User;
using BoutiqueEnLigne.Data;
using System.Linq;

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
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.MotDePasse == motDePasse);
            
            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserEmail", user.Email);
                return RedirectToAction("Index", "Accueil");
            }

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