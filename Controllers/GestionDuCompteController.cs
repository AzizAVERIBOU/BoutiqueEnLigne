using Microsoft.AspNetCore.Mvc;
using BoutiqueEnLigne.Models;
using BoutiqueEnLigne.Models.User;
using BoutiqueEnLigne.Data;
using Microsoft.EntityFrameworkCore;

namespace BoutiqueEnLigne.Controllers
{
    public class GestionDuCompteController : Controller
    {
        private readonly BoutiqueEnLigneContext _context;

        public GestionDuCompteController(BoutiqueEnLigneContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (!HttpContext.Session.GetInt32("UserId").HasValue)
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
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.MotDePasse == motDePasse);
            
            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                return RedirectToAction("Infospersonnelles");
            }
            
            ViewBag.Error = "Email ou mot de passe incorrect";
            return View();
        }

        public IActionResult Inscription()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Inscription(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Vérifier si l'email existe déjà
                    if (_context.Users.Any(u => u.Email == user.Email))
                    {
                        ModelState.AddModelError("Email", "Cet email est déjà utilisé");
                        return View(user);
                    }

                    // Ajouter l'utilisateur à la base de données
                    _context.Users.Add(user);
                    _context.SaveChanges();

                    // Connecter l'utilisateur
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    return RedirectToAction("Infospersonnelles");
                }
                catch (Exception ex)
                {
                    // Log l'erreur
                    ModelState.AddModelError("", "Une erreur est survenue lors de l'inscription. Veuillez réessayer.");
                    return View(user);
                }
            }

            return View(user);
        }

        public IActionResult Infospersonnelles()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Connexion");
            }

            var user = _context.Users.Find(userId.Value);
            if (user == null)
            {
                return RedirectToAction("Connexion");
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult Infospersonnelles(User user)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Connexion");
            }

            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.Find(userId.Value);
                if (existingUser != null)
                {
                    existingUser.Nom = user.Nom;
                    existingUser.Prenom = user.Prenom;
                    existingUser.Email = user.Email;

                    _context.SaveChanges();
                    TempData["Message"] = "Vos informations ont été mises à jour avec succès.";
                    return RedirectToAction(nameof(Infospersonnelles));
                }
            }

            return View(user);
        }

        public IActionResult Commandes()
        {
            if (!HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Connexion");
            }
            return View();
        }

        public IActionResult Adresses()
        {
            if (!HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Connexion");
            }
            return View();
        }

        public IActionResult Securite()
        {
            if (!HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Connexion");
            }
            return View();
        }

        public IActionResult Deconnexion()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Index", "Accueil");
        }
    }
}
