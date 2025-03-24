using Microsoft.AspNetCore.Mvc;

namespace BoutiqueEnLigne.Controllers
{
    public class ProduitController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
