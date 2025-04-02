using Microsoft.AspNetCore.Mvc;
using BoutiqueEnLigne.Services;
using BoutiqueEnLigne.Models;
using Microsoft.Extensions.Logging;

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
    }
}
