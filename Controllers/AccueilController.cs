using Microsoft.AspNetCore.Mvc;
using BoutiqueEnLigne.Services;
using BoutiqueEnLigne.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BoutiqueEnLigne.Controllers
{
    public class AccueilController : Controller
    {
        private readonly ProductApiService _productApiService;
        private readonly ILogger<AccueilController> _logger;

        public AccueilController(
            ProductApiService productApiService,
            ILogger<AccueilController> logger)
        {
            _productApiService = productApiService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                const int productsPerPage = 24;
                var skip = (page - 1) * productsPerPage;

                var products = await _productApiService.GetProductsAsync(skip, productsPerPage);
                var totalProducts = await _productApiService.GetTotalProductsCountAsync();
                var totalPages = (int)Math.Ceiling(totalProducts / (double)productsPerPage);

                var viewModel = new AccueilViewModel
                {
                    Products = products,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    ProductsPerPage = productsPerPage,
                    TotalProducts = totalProducts
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits");
                return View(new AccueilViewModel { Products = new List<Product>() });
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Tentative d'accès aux détails du produit avec l'ID: {Id}", id);

            try
            {
                var produit = await _productApiService.GetProductByIdAsync(id);

                if (produit == null)
                {
                    _logger.LogWarning("Produit non trouvé avec l'ID: {Id}", id);
                    return NotFound();
                }

                _logger.LogInformation("Produit trouvé: {Nom} (ID: {Id})", produit.Nom, produit.ProduitId);
                return View(produit);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des détails du produit {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la récupération des détails du produit.");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AjouterAuPanier(int id, string nom, decimal prix, string image)
        {
            try
            {
                _logger.LogInformation("Début de l'ajout au panier - Produit ID: {Id}, Nom: {Nom}, Prix: {Prix}", id, nom, prix);

                // Récupérer le panier actuel depuis la session
                var panierJson = HttpContext.Session.GetString("Panier");
                _logger.LogInformation("Panier actuel (JSON): {PanierJson}", panierJson);

                var panier = string.IsNullOrEmpty(panierJson) 
                    ? new List<Dictionary<string, JsonElement>>() 
                    : JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(panierJson);

                _logger.LogInformation("Nombre d'articles dans le panier avant ajout: {Count}", panier.Count);

                // Vérifier si le produit est déjà dans le panier
                var produitExistant = panier.FirstOrDefault(p => p["ProduitId"].GetInt32() == id);
                if (produitExistant != null)
                {
                    _logger.LogInformation("Produit déjà dans le panier, mise à jour de la quantité");
                    var quantite = produitExistant["Quantite"].GetInt32() + 1;
                    produitExistant["Quantite"] = JsonSerializer.SerializeToElement(quantite);
                }
                else
                {
                    _logger.LogInformation("Ajout d'un nouveau produit au panier");
                    var nouveauProduit = new Dictionary<string, JsonElement>
                    {
                        ["ProduitId"] = JsonSerializer.SerializeToElement(id),
                        ["Nom"] = JsonSerializer.SerializeToElement(nom),
                        ["Prix"] = JsonSerializer.SerializeToElement(prix),
                        ["Image"] = JsonSerializer.SerializeToElement(image),
                        ["Quantite"] = JsonSerializer.SerializeToElement(1)
                    };
                    panier.Add(nouveauProduit);
                }

                // Sauvegarder le panier mis à jour dans la session
                var nouveauPanierJson = JsonSerializer.Serialize(panier);
                HttpContext.Session.SetString("Panier", nouveauPanierJson);
                _logger.LogInformation("Panier mis à jour avec succès - Nombre d'articles: {Count}", panier.Count);

                return Json(new { success = true, message = "Produit ajouté au panier" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'ajout au panier - Produit ID: {Id}", id);
                return Json(new { success = false, message = "Une erreur est survenue lors de l'ajout au panier" });
            }
        }

        public async Task<IActionResult> Categories()
        {
            try
            {
                _logger.LogInformation("=== Début de l'action Categories ===");
                
                // Récupérer tous les produits
                _logger.LogInformation("Récupération de tous les produits depuis l'API...");
                var products = await _productApiService.GetProductsAsync(0, int.MaxValue);
                _logger.LogInformation("Nombre total de produits récupérés: {Count}", products.Count);
                
                // Grouper par catégorie et compter le nombre de produits
                _logger.LogInformation("Groupement des produits par catégorie...");
                var categories = products
                    .GroupBy(p => p.Categorie)
                    .Select(g => new CategoryViewModel
                    {
                        Nom = g.Key,
                        NombreProduits = g.Count()
                    })
                    .OrderBy(c => c.Nom)
                    .ToList();

                _logger.LogInformation("Nombre de catégories trouvées: {Count}", categories.Count);
                foreach (var category in categories)
                {
                    _logger.LogInformation("Catégorie: {Nom}, Nombre de produits: {NombreProduits}", 
                        category.Nom, category.NombreProduits);
                }

                _logger.LogInformation("=== Fin de l'action Categories ===");
                return View(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des catégories");
                _logger.LogError("Détails de l'erreur: {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Exception interne: {Message}", ex.InnerException.Message);
                }
                return View(new List<CategoryViewModel>());
            }
        }

        public async Task<IActionResult> FiltreCategories(string categorie, int page = 1)
        {
            try
            {
                _logger.LogInformation("=== Début de l'action FiltreCategories ===");
                _logger.LogInformation("Catégorie sélectionnée: {Categorie}, Page: {Page}", categorie, page);

                const int productsPerPage = 24;
                var skip = (page - 1) * productsPerPage;

                // Récupérer tous les produits
                _logger.LogInformation("Récupération des produits depuis l'API...");
                var allProducts = await _productApiService.GetProductsAsync(0, int.MaxValue);
                
                // Filtrer par catégorie
                var filteredProducts = allProducts
                    .Where(p => p.Categorie.Equals(categorie, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                _logger.LogInformation("Nombre total de produits dans la catégorie {Categorie}: {Count}", 
                    categorie, filteredProducts.Count);

                // Pagination
                var products = filteredProducts
                    .Skip(skip)
                    .Take(productsPerPage)
                    .ToList();

                var totalPages = (int)Math.Ceiling(filteredProducts.Count / (double)productsPerPage);

                var viewModel = new AccueilViewModel
                {
                    Products = products,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    ProductsPerPage = productsPerPage,
                    TotalProducts = filteredProducts.Count,
                    CategorieActuelle = categorie
                };

                _logger.LogInformation("=== Fin de l'action FiltreCategories ===");
                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du filtrage des produits par catégorie");
                _logger.LogError("Détails de l'erreur: {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Exception interne: {Message}", ex.InnerException.Message);
                }
                return View("Index", new AccueilViewModel { Products = new List<Product>() });
            }
        }
    }
}
