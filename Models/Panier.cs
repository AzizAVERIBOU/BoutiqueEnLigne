using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoutiqueEnLigne.Models
{
    public class Panier
    {
        [Key]
        public int PanierId { get; set; }

        [Required(ErrorMessage = "Les produits sont requis")] // Message d'erreur si les produits sont requis
        [Display(Name = "Produits")] // Affiche le nom des produits dans le formulaire 
        public List<Product> Produits { get; set; }

        [Required(ErrorMessage = "La quantité est requise")] // Message d'erreur si la quantité est requise
        [Display(Name = "Quantité")] // Affiche le nom de la quantité dans le formulaire 
        [Range(0, int.MaxValue, ErrorMessage = "La quantité doit être positive")] // Message d'erreur si la quantité n'est pas positive
        public int Quantite { get; set; }

        [Required(ErrorMessage = "Le prix total est requis")] // Message d'erreur si le prix total est requis
        [Display(Name = "Prix total")] // Affiche le nom du prix total dans le formulaire 
        [DataType(DataType.Currency)] // Type de données du prix total
        [Range(0, double.MaxValue, ErrorMessage = "Le prix total doit être positif")] // Message d'erreur si le prix total n'est pas positif
        public decimal PrixTotal { get; set; }

        [Required(ErrorMessage = "La date de création est requise")] // Message d'erreur si la date de création est requise
        [Display(Name = "Date de création")] // Affiche le nom de la date de création dans le formulaire 
        public DateTime DateCreation { get; set; }

        [Required(ErrorMessage = "La date de modification est requise")] // Message d'erreur si la date de modification est requise
        [Display(Name = "Date de modification")] // Affiche le nom de la date de modification dans le formulaire 
        public DateTime DateModification { get; set; }

        [Required] // Message d'erreur si le client est requis  
        public int ClientId { get; set; }
        [ForeignKey("ClientId")] // Clé étrangère pour la relation avec le client
        [Display(Name = "Client")] // Affiche le nom du client dans le formulaire 
        public Client Client { get; set; }
    }
}
