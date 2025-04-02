using Microsoft.AspNetCore.Mvc;
using BoutiqueEnLigne.Data;
using BoutiqueEnLigne.Services;
using BoutiqueEnLigne.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoutiqueEnLigne.Controllers
{
    public class AccueilController : Controller
    {
        private readonly BoutiqueEnLigneContext _context;
        private readonly UserApiService _userApiService;
        private readonly ProductApiService _productApiService;
        private readonly ILogger<AccueilController> _logger;
        private const int ITEMS_PER_PAGE = 24;

        public AccueilController(
            BoutiqueEnLigneContext context,
            UserApiService userApiService,
            ProductApiService productApiService,
            ILogger<AccueilController> logger)
        {
            _context = context;
            _userApiService = userApiService;
            _productApiService = productApiService;
            _logger = logger;
        }

        public IActionResult Index(int page = 1)
        {
            var totalProducts = _context.Products.Count();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)ITEMS_PER_PAGE);
            
            // S'assurer que la page demandée est valide
            page = Math.Max(1, Math.Min(page, totalPages));

            var produits = _context.Products
                .OrderByDescending(p => p.DateAjout)
                .Skip((page - 1) * ITEMS_PER_PAGE)
                .Take(ITEMS_PER_PAGE)
                .ToList();
            
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalProducts = totalProducts;
            
            return View(produits);
        }

        public IActionResult Details(int id)
        {
            _logger.LogInformation("Tentative d'accès aux détails du produit avec l'ID: {Id}", id);

            try
            {
                var produit = _context.Products
                    .Include(p => p.Vendeur)
                    .FirstOrDefault(p => p.ProduitId == id);

                if (produit == null)
                {
                    _logger.LogWarning("Produit non trouvé avec l'ID: {Id}", id);
                    return NotFound();
                }

                _logger.LogInformation("Produit trouvé: {Nom} (ID: {Id})", produit.Nom, produit.ProduitId);
                if (produit.Vendeur != null)
                {
                    _logger.LogInformation("Vendeur associé: {Nom} {Prenom}", produit.Vendeur.Nom, produit.Vendeur.Prenom);
                }
                else
                {
                    _logger.LogWarning("Aucun vendeur associé au produit {Id}", id);
                }

                _logger.LogInformation("Redirection vers la vue Details avec le produit {Id}", id);
                return View(produit);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des détails du produit {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la récupération des détails du produit.");
            }
        }

        public IActionResult NettoyerBaseDeDonnees()
        {
            _context.NettoyerBaseDeDonnees();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> PeuplerBaseDeDonnees()
        {
            try
            {
                await _userApiService.PopulateUsersFromApi();
                await _productApiService.PopulateProductsFromApi();
                TempData["Message"] = "La base de données a été peuplée avec succès avec les données de l'API.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Une erreur est survenue lors du peuplement de la base de données : {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
