using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoutiqueEnLigne.Models
{
    public class Commande
    {
        [Key]
        public int CommandeId { get; set; }
        
        [Required(ErrorMessage = "La date de commande est requise")] // Message d'erreur si la date de commande est requise
        [Display(Name = "Date de commande")] // Affiche le nom de la date de commande dans le formulaire 
        public DateTime DateCommande { get; set; }

        [Required(ErrorMessage = "Le prix total est requis")] // Message d'erreur si le prix total est requis
        [Display(Name = "Prix total")] // Affiche le nom du prix total dans le formulaire 
        [DataType(DataType.Currency)] // Type de données du prix total
        [Range(0, double.MaxValue, ErrorMessage = "Le prix total doit être positif")] // Message d'erreur si le prix total n'est pas positif
        public decimal PrixTotal { get; set; }

        [Required(ErrorMessage = "Le statut est requis")] // Message d'erreur si le statut est requis
        [Display(Name = "Statut")] // Affiche le nom du statut dans le formulaire 
        public StatutCommande Statut { get; set; }

        [Required(ErrorMessage = "La validation du paiement est requise")] // Message d'erreur si la validation du paiement est requise
        [Display(Name = "Validation du paiement")] // Affiche le nom de la validation du paiement dans le formulaire 
        public ValidationPaiement Validation { get; set; }

        [Required] // Message d'erreur si le client est requis
        public int ClientId { get; set; }
        [ForeignKey("ClientId")] // Clé étrangère pour la relation avec le client
        [Display(Name = "Client")] // Affiche le nom du client dans le formulaire 
        public Client Client { get; set; }

        [Required] // Message d'erreur si le vendeur est requis
        public int VendeurId { get; set; }
        [ForeignKey("VendeurId")] // Clé étrangère pour la relation avec le vendeur
        [Display(Name = "Vendeur")] // Affiche le nom du vendeur dans le formulaire 
        public Vendeur Vendeur { get; set; }

        [Display(Name = "Produits")] // Affiche le nom des produits dans le formulaire 
        public List<Product> Produits { get; set; }

        [Display(Name = "Facture")] // Affiche le nom de la facture dans le formulaire 
        public Facture Facture { get; set; }

        //public Enum Statut { get; set; }  { enCours; livree, annulee}

        //public Enum Validation { get; set; } : { Payer , NonPayer} 
        
    }
}
