﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BoutiqueEnLigne.Models;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace BoutiqueEnLigne.Controllers
{
    [Authorize]
    public class PaiementController : Controller
    {
        private readonly ILogger<PaiementController> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public PaiementController(ILogger<PaiementController> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
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
