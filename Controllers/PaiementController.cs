using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BoutiqueEnLigne.Models;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Stripe;
using Stripe.Checkout;
using Microsoft.Extensions.Options;

namespace BoutiqueEnLigne.Controllers
{
    [Authorize]
    public class PaiementController : Controller
    {
        private readonly ILogger<PaiementController> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly StripeSettings _stripeSettings;

        public PaiementController(ILogger<PaiementController> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory, IOptions<StripeSettings> stripeSettings)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
            _stripeSettings = stripeSettings.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Panier()
        {
            var panier = HttpContext.Session.GetString("Panier");
            var items = string.IsNullOrEmpty(panier) ? new List<object>() : JsonSerializer.Deserialize<List<object>>(panier);
            
            ViewBag.Items = items;
            ViewBag.Total = items.Sum(i => {
                var item = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(i.ToString());
                return item["Prix"].GetDecimal() * item["Quantite"].GetInt32();
            });
            
            return View();
        }

        [HttpPost]
        public IActionResult ModifierQuantite(int produitId, int delta)
        {
            try
            {
                var panier = HttpContext.Session.GetString("Panier");
                if (!string.IsNullOrEmpty(panier))
                {
                    var items = JsonSerializer.Deserialize<List<object>>(panier);
                    var item = items.FirstOrDefault(i => {
                        var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(i.ToString());
                        return dict["ProduitId"].GetInt32() == produitId;
                    });
                    if (item != null)
                    {
                        var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(item.ToString());
                        var quantite = dict["Quantite"].GetInt32() + delta;
                        if (quantite <= 0)
                        {
                            items.Remove(item);
                        }
                        else
                        {
                            dict["Quantite"] = JsonSerializer.SerializeToElement(quantite);
                            items[items.IndexOf(item)] = JsonSerializer.SerializeToElement(dict);
                        }
                        HttpContext.Session.SetString("Panier", JsonSerializer.Serialize(items));
                        return Json(new { success = true, message = "Quantité modifiée avec succès" });
                    }
                }
                return Json(new { success = false, message = "Produit non trouvé dans le panier" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la modification de la quantité");
                return Json(new { success = false, message = "Une erreur est survenue lors de la modification de la quantité" });
            }
        }

        [HttpPost]
        public IActionResult SupprimerDuPanier(int produitId)
        {
            try
            {
                var panier = HttpContext.Session.GetString("Panier");
                if (!string.IsNullOrEmpty(panier))
                {
                    var items = JsonSerializer.Deserialize<List<object>>(panier);
                    var item = items.FirstOrDefault(i => {
                        var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(i.ToString());
                        return dict["ProduitId"].GetInt32() == produitId;
                    });
                    if (item != null)
                    {
                        items.Remove(item);
                        HttpContext.Session.SetString("Panier", JsonSerializer.Serialize(items));
                        return Json(new { success = true, message = "Produit supprimé du panier" });
                    }
                }
                return Json(new { success = false, message = "Produit non trouvé dans le panier" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression du panier");
                return Json(new { success = false, message = "Une erreur est survenue lors de la suppression du produit" });
            }
        }

        public IActionResult PasserCommande()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ReturnUrl"] = Url.Action("Paiement", "Paiement");
                return RedirectToAction("Login", "Account");
            }

            var panier = HttpContext.Session.GetString("Panier");
            var items = string.IsNullOrEmpty(panier) ? new List<object>() : JsonSerializer.Deserialize<List<object>>(panier);

            if (items.Count == 0)
            {
                TempData["Message"] = "Votre panier est vide.";
                return RedirectToAction("Panier");
            }

            ViewBag.Items = items;
            ViewBag.Total = items.Sum(i => {
                var item = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(i.ToString());
                return item["Prix"].GetDecimal() * item["Quantite"].GetInt32();
            });
            
            return View("Paiement");
        }

        public IActionResult Paiement()
        {
            try
            {
                var panier = HttpContext.Session.GetString("Panier");
                if (string.IsNullOrEmpty(panier))
                {
                    return RedirectToAction("Panier", "GestionDuCompte");
                }

                var items = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(panier);
                ViewBag.Items = items;
                ViewBag.Total = items.Sum(i => i["Prix"].GetDecimal() * i["Quantite"].GetInt32());
                ViewBag.StripePublishableKey = _stripeSettings.PublishableKey;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'accès à la page de paiement");
                return RedirectToAction("Panier", "GestionDuCompte");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheckoutSession()
        {
            try
            {
                _logger.LogInformation("=== Début de la création de la session Stripe ===");
                
                // Récupérer le panier
                var panier = HttpContext.Session.GetString("Panier");
                if (string.IsNullOrEmpty(panier))
                {
                    _logger.LogWarning("Le panier est vide");
                    return BadRequest("Le panier est vide");
                }

                var items = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(panier);
                _logger.LogInformation($"Nombre d'articles dans le panier: {items.Count}");

                // Créer les options de la session Stripe
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = items.Select(item => new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item["Prix"].GetDecimal() * 100), // Convertir en centimes
                            Currency = "eur",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item["Nom"].GetString(),
                                Images = new List<string> { item["Image"].GetString() }
                            }
                        },
                        Quantity = item["Quantite"].GetInt32()
                    }).ToList(),
                    Mode = "payment",
                    SuccessUrl = $"{Request.Scheme}://{Request.Host}/Paiement/Success",
                    CancelUrl = $"{Request.Scheme}://{Request.Host}/Paiement/Cancel"
                };

                _logger.LogInformation("Création de la session Stripe...");
                var service = new SessionService();
                var session = await service.CreateAsync(options);
                _logger.LogInformation($"Session créée avec succès. ID: {session.Id}");

                // Retourner l'URL de la session
                return Json(new { url = session.Url });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de la session Stripe");
                return StatusCode(500, "Une erreur est survenue lors de la création de la session de paiement");
            }
        }

        public IActionResult Success()
        {
            try
            {
                _logger.LogInformation("=== Début de la méthode Success ===");
                
                // Récupérer le panier pour calculer le total
                var panier = HttpContext.Session.GetString("Panier");
                if (!string.IsNullOrEmpty(panier))
                {
                    var items = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(panier);
                    var total = items.Sum(i => i["Prix"].GetDecimal() * i["Quantite"].GetInt32());
                    
                    // Passer les données à la vue
                    ViewBag.Items = items;
                    ViewBag.Total = total;
                    
                    // Vider le panier après un paiement réussi
                    HttpContext.Session.Remove("Panier");
                    
                    _logger.LogInformation($"Paiement réussi - Total: {total}€");
                }
                
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur dans la méthode Success");
                return View();
            }
        }

        public IActionResult Cancel()
        {
            return View();
        }

        public IActionResult Validation()
        {
            return View();
        }

        public IActionResult Factures()
        {
            return View();
        }
    }
}
