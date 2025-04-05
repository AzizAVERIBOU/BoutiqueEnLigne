using Microsoft.AspNetCore.Mvc;
using BoutiqueEnLigne.Services;
using BoutiqueEnLigne.Models;
using Microsoft.Extensions.Logging;

namespace BoutiqueEnLigne.Controllers
{
    public class ProduitController : Controller
    {
        private readonly ProductApiService _productApiService;
        private readonly ILogger<ProduitController> _logger;

        public ProduitController(ProductApiService productApiService, ILogger<ProduitController> logger)
        {
            _productApiService = productApiService;
            _logger = logger;
        }

        public async Task<IActionResult> Recherche(string recherche, int page = 1)
        {
            try
            {
                _logger.LogInformation("=== Début de la recherche ===");
                _logger.LogInformation("Terme de recherche: {Recherche}, Page: {Page}", recherche, page);

                if (string.IsNullOrWhiteSpace(recherche))
                {
                    _logger.LogInformation("Aucun terme de recherche fourni, redirection vers l'accueil");
                    return RedirectToAction("Index", "Accueil");
                }

                const int productsPerPage = 24;
                var skip = (page - 1) * productsPerPage;

                // Récupérer tous les produits
                var products = await _productApiService.GetProductsAsync(0, int.MaxValue);
                
                // Filtrer les produits selon le terme de recherche
                var filteredProducts = products
                    .Where(p => p.Nom.Contains(recherche, StringComparison.OrdinalIgnoreCase) ||
                               p.Description.Contains(recherche, StringComparison.OrdinalIgnoreCase) ||
                               p.Categorie.Contains(recherche, StringComparison.OrdinalIgnoreCase) ||
                               p.Marque.Contains(recherche, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                _logger.LogInformation("Nombre total de résultats trouvés: {Count}", filteredProducts.Count);

                // Pagination
                var paginatedProducts = filteredProducts
                    .Skip(skip)
                    .Take(productsPerPage)
                    .ToList();

                var totalPages = (int)Math.Ceiling(filteredProducts.Count / (double)productsPerPage);

                var viewModel = new AccueilViewModel
                {
                    Products = paginatedProducts,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    ProductsPerPage = productsPerPage,
                    TotalProducts = filteredProducts.Count,
                    TermeRecherche = recherche
                };

                _logger.LogInformation("=== Fin de la recherche ===");
                return View("~/Views/Accueil/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche");
                return View("~/Views/Accueil/Index.cshtml", new AccueilViewModel { Products = new List<Product>() });
            }
        }
    }
} 