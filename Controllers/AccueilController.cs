using Microsoft.AspNetCore.Mvc;
using BoutiqueEnLigne.Data;

namespace BoutiqueEnLigne.Controllers
{
    public class AccueilController : Controller
    {
        private readonly BoutiqueEnLigneContext _context;

        public AccueilController(BoutiqueEnLigneContext context)
        {
            _context = context;
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
    }
}
