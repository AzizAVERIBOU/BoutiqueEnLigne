using Microsoft.AspNetCore.Mvc;
using BoutiqueEnLigne.Models;
using BoutiqueEnLigne.Models.User;
using BoutiqueEnLigne.Data;
using Microsoft.EntityFrameworkCore;
using BoutiqueEnLigne.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BoutiqueEnLigne.Controllers
{
    [Authorize]
    public class GestionDuCompteController : Controller
    {
        private readonly BoutiqueEnLigneContext _context;
        private readonly UserApiService _userApiService;

        public GestionDuCompteController(BoutiqueEnLigneContext context, UserApiService userApiService)
        {
            _context = context;
            _userApiService = userApiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Connexion()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Connexion(string email, string motDePasse)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(motDePasse))
            {
                ModelState.AddModelError("", "Veuillez remplir tous les champs");
                return View();
            }

            var user = await _userApiService.AuthenticateUser(email, motDePasse);
            if (user == null)
            {
                ModelState.AddModelError("", "Email ou mot de passe incorrect");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Infospersonnelles");
        }

        [AllowAnonymous]
        public IActionResult Inscription()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
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
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(ClaimTypes.Role, user.Role.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                    };

                    HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties).Wait();

                    return RedirectToAction("Infospersonnelles");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Une erreur est survenue lors de l'inscription. Veuillez réessayer.");
                    return View(user);
                }
            }

            return View(user);
        }

        public IActionResult Infospersonnelles()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Connexion");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == userId);
            if (user == null)
            {
                return RedirectToAction("Connexion");
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult Infospersonnelles(User user)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Connexion");
            }

            var userId = int.Parse(userIdClaim.Value);
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.Find(userId);
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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Connexion");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == userId);
            if (user == null)
            {
                return RedirectToAction("Connexion");
            }

            return View();
        }

        public async Task<IActionResult> Adresses()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Connexion");
            }

            var user = await _context.Users
                .Include(u => u.Address)
                    .ThenInclude(a => a.Coordinates)
                .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            if (user == null)
            {
                return RedirectToAction("Connexion");
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Adresses(Address address)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Connexion");
            }

            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .Include(u => u.Address)
                    .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

                if (user != null)
                {
                    if (user.Address == null)
                    {
                        // Créer une nouvelle adresse
                        address.UserId = user.Id;
                        _context.Addresses.Add(address);
                    }
                    else
                    {
                        // Mettre à jour l'adresse existante
                        user.Address.Address1 = address.Address1;
                        user.Address.City = address.City;
                        user.Address.PostalCode = address.PostalCode;
                        user.Address.State = address.State;
                        _context.Addresses.Update(user.Address);
                    }

                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Votre adresse a été mise à jour avec succès.";
                    return RedirectToAction(nameof(Adresses));
                }
            }

            // En cas d'erreur, récupérer l'utilisateur complet pour la vue
            var userWithAddress = await _context.Users
                .Include(u => u.Address)
                    .ThenInclude(a => a.Coordinates)
                .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            return View(userWithAddress);
        }

        public IActionResult Securite()
        {
            return View();
        }

        public async Task<IActionResult> Deconnexion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Accueil");
        }
    }
}
