using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilisateur = BoutiqueEnLigne.Models.User.User;

namespace BoutiqueEnLigne.Models
{
    /// <summary>
    /// Produit unique du catalogue. Le vendeur écrit ici ; la vitrine lira cette table à l'étape suivante.
    /// </summary>
    public class ProduitVendeur
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom du produit est requis")]
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "La description est requise")]
        [StringLength(500, ErrorMessage = "La description ne peut pas dépasser 500 caractères")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Le prix est requis")]
        [Range(0.01, 10000, ErrorMessage = "Le prix doit être compris entre 0.01 et 10000")]
        public decimal Prix { get; set; }

        [Required(ErrorMessage = "La quantité en stock est requise")]
        [Range(0, 1000, ErrorMessage = "La quantité doit être comprise entre 0 et 1000")]
        public int QuantiteEnStock { get; set; }

        [Required(ErrorMessage = "La catégorie est requise")]
        [StringLength(100, ErrorMessage = "La catégorie ne peut pas dépasser 100 caractères")]
        public string Categorie { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        [Required]
        public int VendeurId { get; set; }

        [ForeignKey(nameof(VendeurId))]
        public Utilisateur? Vendeur { get; set; }

        public DateTime DateAjout { get; set; } = DateTime.Now;

        public bool EstActif { get; set; } = true;
    }
} 