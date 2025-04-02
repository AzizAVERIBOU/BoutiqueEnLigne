using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using BoutiqueEnLigne.Models;
using BoutiqueEnLigne.Services;
using BoutiqueEnLigne.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoutiqueEnLigne.Controllers
{
    public class AccountController : Controller
    {
        private readonly BoutiqueEnLigneContext _context;
        private readonly AuthApiService _authApiService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            BoutiqueEnLigneContext context,
            AuthApiService authApiService,
            ILogger<AccountController> logger)
        {
            _context = context;
            _authApiService = authApiService;
            _logger = logger;
        }

        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                try
                {
                    // Tenter la connexion via l'API
                    var loginResponse = await _authApiService.LoginAsync(model.Email, model.Password);

                    if (loginResponse != null && loginResponse.Token != null)
                    {
                        // Récupérer les informations complètes de l'utilisateur
                        var user = await _authApiService.GetUserByIdAsync(loginResponse.Id);

                        if (user != null)
                        {
                            // Vérifier si l'utilisateur existe déjà dans la base de données
                            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

                            if (existingUser == null)
                            {
                                // Ajouter l'utilisateur à la base de données
                                _context.Users.Add(user);
                                await _context.SaveChangesAsync();
                                _logger.LogInformation("Nouvel utilisateur ajouté à la base de données: {Email}", user.Email);
                            }

                            // Créer les claims pour l'authentification
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                                new Claim(ClaimTypes.Email, user.Email),
                                new Claim(ClaimTypes.Name, $"{user.Prenom} {user.Nom}"),
                                new Claim(ClaimTypes.Role, user.Role.ToString())
                            };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var authProperties = new AuthenticationProperties
                            {
                                IsPersistent = model.RememberMe,
                                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                            };

                            await HttpContext.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(claimsIdentity),
                                authProperties);

                            _logger.LogInformation("Utilisateur connecté avec succès: {Email}", user.Email);

                            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }

                            return RedirectToAction("Index", "Accueil");
                        }
                    }

                    ModelState.AddModelError(string.Empty, "Tentative de connexion invalide.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur lors de la tentative de connexion pour l'utilisateur: {Email}", model.Email);
                    ModelState.AddModelError(string.Empty, "Une erreur est survenue lors de la tentative de connexion.");
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Accueil");
        }
    }
} 