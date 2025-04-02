using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BoutiqueEnLigne.Controllers
{
    [Authorize]
    public class PaiementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Panier()
        {
            return View();
        }

        public IActionResult Paiement()
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
