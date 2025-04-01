using Microsoft.AspNetCore.Mvc;
using BoutiqueEnLigne.Data;
using BoutiqueEnLigne.Services;

namespace BoutiqueEnLigne.Controllers
{
    public class AccueilController : Controller
    {
        private readonly BoutiqueEnLigneContext _context;
        private readonly UserApiService _userApiService;
        private readonly ProductApiService _productApiService;

        public AccueilController(
            BoutiqueEnLigneContext context,
            UserApiService userApiService,
            ProductApiService productApiService)
        {
            _context = context;
            _userApiService = userApiService;
            _productApiService = productApiService;
        }

        public IActionResult Index()
        {
            return View();
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
