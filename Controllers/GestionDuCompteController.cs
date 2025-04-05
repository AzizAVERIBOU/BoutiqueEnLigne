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
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace BoutiqueEnLigne.Controllers
{
    public class GestionDuCompteController : Controller
    {
        private readonly BoutiqueEnLigneContext _context;
        private readonly UserApiService _userApiService;
        private readonly ILogger<GestionDuCompteController> _logger;

        public GestionDuCompteController(BoutiqueEnLigneContext context, UserApiService userApiService, ILogger<GestionDuCompteController> logger)
        {
            _context = context;
            _userApiService = userApiService;
            _logger = logger;
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
            try
            {
                Console.WriteLine("=== Début de la méthode Connexion ===");
                Console.WriteLine($"Tentative de connexion pour l'email: {email}");

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(motDePasse))
                {
                    Console.WriteLine("Email ou mot de passe vide");
                    ModelState.AddModelError("", "Veuillez remplir tous les champs");
                    return View();
                }

                // 1. Vérifier d'abord dans la base de données locale
                Console.WriteLine("Recherche de l'utilisateur dans la base locale...");
                var localUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (localUser != null)
                {
                    Console.WriteLine($"Utilisateur trouvé dans la base locale: Email={localUser.Email}, Id={localUser.Id}");
                    Console.WriteLine($"Mot de passe stocké: {localUser.MotDePasse}");
                    Console.WriteLine($"Mot de passe fourni: {motDePasse}");
                    
                    // Vérifier le mot de passe
                    if (localUser.MotDePasse == motDePasse) // Note: En production, utiliser un hash
                    {
                        Console.WriteLine("Mot de passe correct pour l'utilisateur local");
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, localUser.Email),
                            new Claim(ClaimTypes.Role, localUser.Role.ToString()),
                            new Claim(ClaimTypes.NameIdentifier, localUser.Id.ToString())
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

                        Console.WriteLine("Connexion réussie pour l'utilisateur local");
                        return RedirectToAction("Infospersonnelles");
                    }
                    else
                    {
                        Console.WriteLine("Mot de passe incorrect pour l'utilisateur local");
                        ModelState.AddModelError("", "Email ou mot de passe incorrect");
                        return View();
                    }
                }

                Console.WriteLine("Utilisateur non trouvé localement, tentative avec l'API");
                // 2. Si l'utilisateur n'est pas dans la base locale, essayer l'API
                var apiUser = await _userApiService.AuthenticateUser(email, motDePasse);
                
                if (apiUser != null)
                {
                    Console.WriteLine("Utilisateur trouvé dans l'API");
                    Console.WriteLine("Création du nouvel utilisateur dans la base locale...");
                    
                    // Vérifier si l'utilisateur n'existe pas déjà dans la base locale
                    var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == apiUser.Email);
                    if (existingUser != null)
                    {
                        Console.WriteLine("L'utilisateur existe déjà dans la base locale");
                        // Créer les claims pour l'authentification
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, existingUser.Email),
                            new Claim(ClaimTypes.Role, existingUser.Role.ToString()),
                            new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString())
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

                        Console.WriteLine("Connexion réussie pour l'utilisateur existant");
                        return RedirectToAction("Infospersonnelles");
                    }

                    // Créer le nouvel utilisateur
                    var newUser = new User
                    {
                        Email = apiUser.Email,
                        MotDePasse = motDePasse,
                        Nom = apiUser.Nom,
                        Prenom = apiUser.Prenom,
                        DateInscription = DateTime.Now,
                        DerniereConnexion = DateTime.Now,
                        EstActif = true,
                        InscritNewsletter = false,
                        NotificationsEmail = false,
                        Image = apiUser.Image ?? "default-avatar.png",
                        Role = RoleUtilisateur.Client,
                        Username = apiUser.Username ?? apiUser.Email.Split('@')[0],
                        Genre = "other",
                        Telephone = "",
                        DateNaissance = null
                    };

                    Console.WriteLine("Données de l'utilisateur à créer:");
                    Console.WriteLine($"- Email: {newUser.Email}");
                    Console.WriteLine($"- Nom: {newUser.Nom}");
                    Console.WriteLine($"- Prénom: {newUser.Prenom}");
                    Console.WriteLine($"- Username: {newUser.Username}");
                    Console.WriteLine($"- Genre: {newUser.Genre}");
                    Console.WriteLine($"- Téléphone: {newUser.Telephone}");
                    Console.WriteLine($"- Date de naissance: {newUser.DateNaissance}");
                    Console.WriteLine($"- Image: {newUser.Image}");
                    Console.WriteLine($"- Role: {newUser.Role}");

                    Console.WriteLine("Ajout de l'utilisateur au contexte...");
                    _context.Users.Add(newUser);
                    Console.WriteLine("Sauvegarde des changements...");
                    await _context.SaveChangesAsync();

                    // Créer les claims pour l'authentification
                    var newClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, newUser.Email),
                        new Claim(ClaimTypes.Role, newUser.Role.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, newUser.Id.ToString())
                    };

                    var newClaimsIdentity = new ClaimsIdentity(newClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var newAuthProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(newClaimsIdentity),
                        newAuthProperties);

                    Console.WriteLine("Connexion réussie pour le nouvel utilisateur");
                    return RedirectToAction("Infospersonnelles");
                }

                Console.WriteLine("Échec de la connexion - utilisateur non trouvé");
                ModelState.AddModelError("", "Email ou mot de passe incorrect");
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur dans la méthode Connexion: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    Console.WriteLine($"Inner stack trace: {ex.InnerException.StackTrace}");
                }
                ModelState.AddModelError("", "Une erreur est survenue lors de la connexion");
                return View();
            }
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

                    // Initialiser les nouveaux champs
                    user.Username = user.Username ?? user.Email.Split('@')[0];
                    user.Genre = user.Genre ?? "other";
                    user.Telephone = user.Telephone ?? "";
                    user.DateInscription = DateTime.Now;
                    user.DerniereConnexion = DateTime.Now;
                    user.EstActif = true;
                    user.InscritNewsletter = false;
                    user.NotificationsEmail = false;
                    user.Role = RoleUtilisateur.Client;

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

        [Authorize]
        public async Task<IActionResult> Infospersonnelles()
        {
            try
            {
                Console.WriteLine("=== Début de la méthode Infospersonnelles ===");
                var userEmail = User.Identity.Name;
                Console.WriteLine($"Email de l'utilisateur connecté: {userEmail}");

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == userEmail);

                if (user == null)
                {
                    Console.WriteLine("Utilisateur non trouvé dans la base de données");
                    return RedirectToAction("Connexion");
                }

                Console.WriteLine("Informations de l'utilisateur trouvé:");
                Console.WriteLine($"- ID: {user.Id}");
                Console.WriteLine($"- Email: {user.Email}");
                Console.WriteLine($"- Nom: {user.Nom}");
                Console.WriteLine($"- Prénom: {user.Prenom}");
                Console.WriteLine($"- Username: {user.Username}");
                Console.WriteLine($"- Genre: {user.Genre}");
                Console.WriteLine($"- Téléphone: {user.Telephone}");
                Console.WriteLine($"- Date de naissance: {user.DateNaissance}");
                Console.WriteLine($"- Image: {user.Image}");
                Console.WriteLine($"- Role: {user.Role}");
                Console.WriteLine($"- Date d'inscription: {user.DateInscription}");
                Console.WriteLine($"- Dernière connexion: {user.DerniereConnexion}");
                Console.WriteLine($"- Est actif: {user.EstActif}");
                Console.WriteLine($"- Inscrit newsletter: {user.InscritNewsletter}");
                Console.WriteLine($"- Notifications email: {user.NotificationsEmail}");

                // Vérifier que les valeurs ne sont pas null
                user.Nom = user.Nom ?? string.Empty;
                user.Prenom = user.Prenom ?? string.Empty;
                user.Email = user.Email ?? string.Empty;
                user.Image = user.Image ?? "default-avatar.png";
                user.Username = user.Username ?? string.Empty;
                user.Genre = user.Genre ?? string.Empty;
                user.Telephone = user.Telephone ?? string.Empty;

                Console.WriteLine("=== Fin de la méthode Infospersonnelles ===");
                return View(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur dans Infospersonnelles: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    Console.WriteLine($"Inner stack trace: {ex.InnerException.StackTrace}");
                }
                return RedirectToAction("Connexion");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Infospersonnelles(User user)
        {
            try
            {
                Console.WriteLine("=== Début de la méthode POST Infospersonnelles ===");
                Console.WriteLine("Données reçues du formulaire:");
                Console.WriteLine($"- Email: {user.Email}");
                Console.WriteLine($"- Nom: {user.Nom}");
                Console.WriteLine($"- Prénom: {user.Prenom}");
                Console.WriteLine($"- Username: {user.Username}");
                Console.WriteLine($"- Genre: {user.Genre}");
                Console.WriteLine($"- Téléphone: {user.Telephone}");
                Console.WriteLine($"- Date de naissance: {user.DateNaissance}");
                Console.WriteLine($"- Est actif: {user.EstActif}");
                Console.WriteLine($"- Inscrit newsletter: {user.InscritNewsletter}");
                Console.WriteLine($"- Notifications email: {user.NotificationsEmail}");

                var currentUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == User.Identity.Name);

                if (currentUser == null)
                {
                    Console.WriteLine("Utilisateur actuel non trouvé dans la base de données");
                    return RedirectToAction("Connexion");
                }

                Console.WriteLine("Mise à jour des informations de l'utilisateur...");
                // Mise à jour des informations modifiables
                currentUser.Prenom = user.Prenom;
                currentUser.Nom = user.Nom;
                currentUser.Email = user.Email;
                currentUser.Username = user.Username;
                currentUser.Genre = user.Genre;
                currentUser.Telephone = user.Telephone;
                currentUser.DateNaissance = user.DateNaissance;
                currentUser.InscritNewsletter = user.InscritNewsletter;
                currentUser.NotificationsEmail = user.NotificationsEmail;
                currentUser.EstActif = user.EstActif;
                currentUser.DerniereConnexion = DateTime.Now;

                await _context.SaveChangesAsync();
                Console.WriteLine("Informations mises à jour avec succès");
                TempData["Message"] = "Vos informations ont été mises à jour avec succès.";

                Console.WriteLine("=== Fin de la méthode POST Infospersonnelles ===");
                return RedirectToAction("Infospersonnelles");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur dans la mise à jour des informations: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    Console.WriteLine($"Inner stack trace: {ex.InnerException.StackTrace}");
                }
                ModelState.AddModelError("", "Une erreur est survenue lors de la mise à jour de vos informations.");
                return View(user);
            }
        }

        [Authorize]
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

        [Authorize]
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
        [Authorize]
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

        [Authorize]
        public IActionResult Securite()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Deconnexion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Accueil");
        }

        [AllowAnonymous]
        public IActionResult Panier()
        {
            try
            {
                Console.WriteLine("=== Début de la méthode Panier ===");
                var panier = HttpContext.Session.GetString("Panier");
                Console.WriteLine($"Contenu brut du panier: {panier}");

                List<Dictionary<string, JsonElement>> items;

                if (string.IsNullOrEmpty(panier))
                {
                    Console.WriteLine("Le panier est vide, initialisation d'un nouveau panier");
                    items = new List<Dictionary<string, JsonElement>>();
                }
                else
                {
                    try
                    {
                        items = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(panier);
                        Console.WriteLine($"Nombre d'articles dans le panier: {items.Count}");
                        foreach (var item in items)
                        {
                            Console.WriteLine($"Article: ID={item["ProduitId"].GetInt32()}, Nom={item["Nom"].GetString()}, Quantité={item["Quantite"].GetInt32()}");
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Erreur de désérialisation: {ex.Message}");
                        items = new List<Dictionary<string, JsonElement>>();
                        HttpContext.Session.SetString("Panier", JsonSerializer.Serialize(items));
                    }
                }

                ViewBag.Items = items;
                ViewBag.Total = items.Sum(i => i["Prix"].GetDecimal() * i["Quantite"].GetInt32());
                Console.WriteLine($"Total du panier: {ViewBag.Total}");
                Console.WriteLine("=== Fin de la méthode Panier ===");

                return View("~/Views/Paiement/Panier.cshtml");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur dans la méthode Panier: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                var items = new List<Dictionary<string, JsonElement>>();
                HttpContext.Session.SetString("Panier", JsonSerializer.Serialize(items));
                ViewBag.Items = items;
                ViewBag.Total = 0;
                return View("~/Views/Paiement/Panier.cshtml");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult AjouterAuPanier(int produitId, string nom, decimal prix, int quantite = 1)
        {
            try
            {
                Console.WriteLine("=== Début de la méthode AjouterAuPanier ===");
                Console.WriteLine($"Paramètres: produitId={produitId}, nom={nom}, prix={prix}, quantite={quantite}");

                var panier = HttpContext.Session.GetString("Panier");
                Console.WriteLine($"Contenu brut du panier avant ajout: {panier}");

                List<Dictionary<string, JsonElement>> items;

                if (string.IsNullOrEmpty(panier))
                {
                    Console.WriteLine("Le panier est vide, initialisation d'un nouveau panier");
                    items = new List<Dictionary<string, JsonElement>>();
                }
                else
                {
                    try
                    {
                        items = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(panier);
                        Console.WriteLine($"Nombre d'articles dans le panier avant ajout: {items.Count}");
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Erreur de désérialisation: {ex.Message}");
                        items = new List<Dictionary<string, JsonElement>>();
                    }
                }

                var existingItem = items.FirstOrDefault(i => i["ProduitId"].GetInt32() == produitId);
                if (existingItem != null)
                {
                    var newQuantite = existingItem["Quantite"].GetInt32() + quantite;
                    Console.WriteLine($"Produit existant trouvé, nouvelle quantité: {newQuantite}");
                    items.Remove(existingItem);
                    items.Add(new Dictionary<string, JsonElement>
                    {
                        ["ProduitId"] = JsonSerializer.SerializeToElement(produitId),
                        ["Nom"] = JsonSerializer.SerializeToElement(nom),
                        ["Prix"] = JsonSerializer.SerializeToElement(prix),
                        ["Quantite"] = JsonSerializer.SerializeToElement(newQuantite)
                    });
                }
                else
                {
                    Console.WriteLine("Ajout d'un nouveau produit au panier");
                    items.Add(new Dictionary<string, JsonElement>
                    {
                        ["ProduitId"] = JsonSerializer.SerializeToElement(produitId),
                        ["Nom"] = JsonSerializer.SerializeToElement(nom),
                        ["Prix"] = JsonSerializer.SerializeToElement(prix),
                        ["Quantite"] = JsonSerializer.SerializeToElement(quantite)
                    });
                }

                var panierSerialise = JsonSerializer.Serialize(items);
                Console.WriteLine($"Contenu du panier après ajout: {panierSerialise}");
                HttpContext.Session.SetString("Panier", panierSerialise);

                var totalItems = items.Sum(i => i["Quantite"].GetInt32());
                Console.WriteLine($"Nombre total d'articles dans le panier: {totalItems}");
                Console.WriteLine("=== Fin de la méthode AjouterAuPanier ===");

                return Json(new { 
                    success = true, 
                    message = "Produit ajouté au panier avec succès", 
                    count = totalItems,
                    debug = new {
                        panierAvant = panier,
                        panierApres = panierSerialise,
                        nombreArticles = totalItems
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur dans la méthode AjouterAuPanier: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return Json(new { 
                    success = false, 
                    message = "Une erreur est survenue lors de l'ajout au panier",
                    debug = new {
                        erreur = ex.Message,
                        stackTrace = ex.StackTrace
                    }
                });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult SupprimerDuPanier([FromBody] JsonElement data)
        {
            try
            {
                Console.WriteLine("=== Début de la méthode SupprimerDuPanier ===");
                int produitId = data.GetProperty("produitId").GetInt32();
                Console.WriteLine($"Produit à supprimer: {produitId}");

                var panier = HttpContext.Session.GetString("Panier");
                Console.WriteLine($"Contenu brut du panier avant suppression: {panier}");

                if (string.IsNullOrEmpty(panier))
                {
                    Console.WriteLine("Le panier est vide");
                    return Json(new { 
                        success = false, 
                        message = "Le panier est vide",
                        debug = new {
                            panier = panier
                        }
                    });
                }

                var items = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(panier);
                Console.WriteLine($"Nombre d'articles dans le panier avant suppression: {items.Count}");

                var itemToRemove = items.FirstOrDefault(i => i["ProduitId"].GetInt32() == produitId);
                if (itemToRemove == null)
                {
                    Console.WriteLine($"Produit {produitId} non trouvé dans le panier");
                    return Json(new { 
                        success = false, 
                        message = "Le produit n'existe pas dans le panier",
                        debug = new {
                            produitId = produitId,
                            panier = panier
                        }
                    });
                }

                Console.WriteLine($"Produit trouvé: ID={itemToRemove["ProduitId"].GetInt32()}, Nom={itemToRemove["Nom"].GetString()}");
                items.Remove(itemToRemove);

                var panierSerialise = JsonSerializer.Serialize(items);
                Console.WriteLine($"Contenu du panier après suppression: {panierSerialise}");
                HttpContext.Session.SetString("Panier", panierSerialise);

                Console.WriteLine("=== Fin de la méthode SupprimerDuPanier ===");
                return Json(new { 
                    success = true, 
                    message = "Produit supprimé du panier avec succès",
                    debug = new {
                        panierAvant = panier,
                        panierApres = panierSerialise
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur dans la méthode SupprimerDuPanier: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return Json(new { 
                    success = false, 
                    message = "Une erreur est survenue lors de la suppression du produit",
                    debug = new {
                        erreur = ex.Message,
                        stackTrace = ex.StackTrace
                    }
                });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ModifierQuantite(int produitId, int delta)
        {
            try
            {
                var panier = HttpContext.Session.GetString("Panier");
                if (string.IsNullOrEmpty(panier))
                {
                    return Json(new { success = false, message = "Le panier est vide" });
                }

                var items = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(panier);
                var itemToUpdate = items.FirstOrDefault(i => i["ProduitId"].GetInt32() == produitId);

                if (itemToUpdate == null)
                {
                    return Json(new { success = false, message = "Le produit n'existe pas dans le panier" });
                }

                var currentQuantite = itemToUpdate["Quantite"].GetInt32();
                var newQuantite = currentQuantite + delta;

                if (newQuantite < 1)
                {
                    return Json(new { success = false, message = "La quantité ne peut pas être inférieure à 1" });
                }

                // Mettre à jour la quantité
                items.Remove(itemToUpdate);
                items.Add(new Dictionary<string, JsonElement>
                {
                    ["ProduitId"] = JsonSerializer.SerializeToElement(produitId),
                    ["Nom"] = itemToUpdate["Nom"],
                    ["Prix"] = itemToUpdate["Prix"],
                    ["Quantite"] = JsonSerializer.SerializeToElement(newQuantite)
                });

                HttpContext.Session.SetString("Panier", JsonSerializer.Serialize(items));

                return Json(new { success = true, message = "Quantité mise à jour avec succès" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Une erreur est survenue lors de la modification de la quantité" });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult PasserCommande()
        {
            try
            {
                Console.WriteLine("=== Début de la méthode PasserCommande ===");
                
                // Vérifier si l'utilisateur est connecté
                if (!User.Identity.IsAuthenticated)
                {
                    Console.WriteLine("Utilisateur non connecté, redirection vers la page de connexion");
                    return Json(new { 
                        success = false, 
                        message = "Vous devez être connecté pour passer une commande",
                        redirectTo = "/GestionDuCompte/Connexion"
                    });
                }

                // Vérifier si le panier n'est pas vide
                var panier = HttpContext.Session.GetString("Panier");
                if (string.IsNullOrEmpty(panier))
                {
                    Console.WriteLine("Le panier est vide");
                    return Json(new { 
                        success = false, 
                        message = "Votre panier est vide",
                        redirectTo = "/"
                    });
                }

                // Récupérer les informations de l'utilisateur connecté
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                Console.WriteLine($"Utilisateur connecté: ID={userId}, Email={userEmail}");

                // Ici, nous pourrions ajouter la logique pour créer la commande
                // Pour l'instant, nous redirigeons simplement vers la page de paiement
                Console.WriteLine("Redirection vers la page de paiement");
                return Json(new { 
                    success = true, 
                    message = "Redirection vers la page de paiement",
                    redirectTo = "/Paiement/Index"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur dans la méthode PasserCommande: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return Json(new { 
                    success = false, 
                    message = "Une erreur est survenue lors du traitement de votre commande",
                    debug = new {
                        erreur = ex.Message,
                        stackTrace = ex.StackTrace
                    }
                });
            }
        }

        public IActionResult Favoris()
        {
            try
            {
                _logger.LogInformation("Accès à la page des favoris");
                var favoris = HttpContext.Session.GetString("Favoris");
                var items = string.IsNullOrEmpty(favoris) ? new List<Dictionary<string, JsonElement>>() : 
                    System.Text.Json.JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(favoris);

                var viewModel = new FavorisViewModel
                {
                    Items = items
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'accès à la page des favoris");
                return View(new FavorisViewModel { Items = new List<Dictionary<string, JsonElement>>() });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult AjouterAuxFavoris(int produitId, string nom, decimal prix, string image, string description)
        {
            try
            {
                _logger.LogInformation("=== Début de la méthode AjouterAuxFavoris ===");
                _logger.LogInformation($"Paramètres: produitId={produitId}, nom={nom}, prix={prix}, image={image}, description={description}");

                var favoris = HttpContext.Session.GetString("Favoris");
                _logger.LogInformation($"Contenu brut des favoris avant ajout:\n{favoris}");

                List<dynamic> favorisList;
                if (string.IsNullOrEmpty(favoris))
                {
                    favorisList = new List<dynamic>();
                }
                else
                {
                    var jsonElements = JsonSerializer.Deserialize<List<JsonElement>>(favoris);
                    favorisList = jsonElements.Select(element => new
                    {
                        ProduitId = element.GetProperty("ProduitId").GetInt32(),
                        Nom = element.GetProperty("Nom").GetString(),
                        Prix = element.GetProperty("Prix").GetDecimal(),
                        Image = element.GetProperty("Image").GetString(),
                        Description = element.GetProperty("Description").GetString()
                    }).ToList<dynamic>();
                }

                // Vérifier si le produit est déjà dans les favoris
                if (!favorisList.Any(p => ((dynamic)p).ProduitId == produitId))
                {
                    favorisList.Add(new
                    {
                        ProduitId = produitId,
                        Nom = nom,
                        Prix = prix,
                        Image = image,
                        Description = description
                    });

                    var updatedFavoris = JsonSerializer.Serialize(favorisList);
                    HttpContext.Session.SetString("Favoris", updatedFavoris);
                    _logger.LogInformation($"Contenu des favoris après ajout:\n{updatedFavoris}");
                    _logger.LogInformation($"Nombre total d'articles dans les favoris: {favorisList.Count}");
                }
                else
                {
                    _logger.LogInformation("Le produit est déjà dans les favoris");
                }

                _logger.LogInformation("=== Fin de la méthode AjouterAuxFavoris ===");
                return Json(new { success = true, message = "Produit ajouté aux favoris avec succès" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de l'ajout aux favoris: {ex.Message}");
                return Json(new { success = false, message = "Erreur lors de l'ajout aux favoris" });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult RetirerDesFavoris(int id)
        {
            _logger.LogInformation("=== Début de la méthode RetirerDesFavoris ===");
            _logger.LogInformation("Produit à retirer: {Id}", id);

            try
            {
                // Récupérer les favoris actuels
                var favorisJson = HttpContext.Session.GetString("Favoris");
                _logger.LogInformation("Contenu brut des favoris avant retrait: {Favoris}", favorisJson);

                if (string.IsNullOrEmpty(favorisJson))
                {
                    _logger.LogWarning("Aucun favoris trouvé");
                    return Json(new { success = false, message = "Aucun favoris trouvé" });
                }

                // Désérialiser les favoris
                var favoris = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(favorisJson);
                
                // Trouver et retirer le produit
                var produit = favoris.FirstOrDefault(f => f["ProduitId"].GetInt32() == id);
                if (produit != null)
                {
                    favoris.Remove(produit);
                    
                    // Mettre à jour la session
                    var newFavorisJson = JsonSerializer.Serialize(favoris);
                    HttpContext.Session.SetString("Favoris", newFavorisJson);
                    
                    _logger.LogInformation("Contenu des favoris après retrait: {Favoris}", newFavorisJson);
                    _logger.LogInformation("=== Fin de la méthode RetirerDesFavoris ===");
                    
                    return Json(new { success = true, message = "Produit retiré des favoris avec succès" });
                }
                else
                {
                    _logger.LogWarning("Produit non trouvé dans les favoris");
                    return Json(new { success = false, message = "Produit non trouvé dans les favoris" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du retrait des favoris");
                return Json(new { success = false, message = "Une erreur est survenue lors du retrait des favoris" });
            }
        }
    }
}
