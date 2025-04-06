using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BoutiqueEnLigne.Models;
using BoutiqueEnLigne.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BoutiqueEnLigne.Controllers
{
    [Authorize(Roles = "Vendeur")]
    public class VendeurController : Controller
    {
        private readonly BoutiqueEnLigneContext _context;

        public VendeurController(BoutiqueEnLigneContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vendeurId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var produits = await _context.ProduitsVendeurs
                .Where(p => p.VendeurId == vendeurId)
                .ToListAsync();
            return View(produits);
        }

        public IActionResult AjouterProduit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AjouterProduit(ProduitVendeur produit)
        {
            if (ModelState.IsValid)
            {
                var vendeurId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                produit.VendeurId = vendeurId;
                produit.DateAjout = DateTime.Now;
                produit.EstActif = true;

                _context.Add(produit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produit);
        }

        public async Task<IActionResult> ModifierProduit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendeurId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var produit = await _context.ProduitsVendeurs
                .FirstOrDefaultAsync(p => p.Id == id && p.VendeurId == vendeurId);

            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModifierProduit(int id, ProduitVendeur produit)
        {
            if (id != produit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var vendeurId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                    if (produit.VendeurId != vendeurId)
                    {
                        return Forbid();
                    }

                    _context.Update(produit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduitExists(produit.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(produit);
        }

        public async Task<IActionResult> SupprimerProduit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendeurId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var produit = await _context.ProduitsVendeurs
                .FirstOrDefaultAsync(p => p.Id == id && p.VendeurId == vendeurId);

            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        [HttpPost, ActionName("SupprimerProduit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SupprimerProduitConfirm(int id)
        {
            var vendeurId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var produit = await _context.ProduitsVendeurs
                .FirstOrDefaultAsync(p => p.Id == id && p.VendeurId == vendeurId);

            if (produit != null)
            {
                _context.ProduitsVendeurs.Remove(produit);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Ventes()
        {
            var vendeurId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var ventes = await _context.Commandes
                .Include(c => c.Produits)
                .Where(c => c.Produits.Any(p => p.VendeurId == vendeurId))
                .ToListAsync();

            return View(ventes);
        }

        private bool ProduitExists(int id)
        {
            return _context.ProduitsVendeurs.Any(e => e.Id == id);
        }
    }
} 