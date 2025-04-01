using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoutiqueEnLigne.Models
{
    public class Facture
    {
        [Key]
        public int FactureId { get; set; }

        [Required(ErrorMessage = "Les produits sont requis")] // Message d'erreur si les produits sont requis
        [Display(Name = "Produits")] // Affiche le nom des produits dans le formulaire 
        public List<Product> Produits { get; set; }

        [Required(ErrorMessage = "Le prix total est requis")] // Message d'erreur si le prix total est requis
        [Display(Name = "Prix total")] // Affiche le nom du prix total dans le formulaire 
        [DataType(DataType.Currency)] // Type de données du prix total
        [Range(0, double.MaxValue, ErrorMessage = "Le prix total doit être positif")] // Message d'erreur si le prix total n'est pas positif
        public decimal PrixTotal { get; set; }

        [Required(ErrorMessage = "La date de facturation est requise")] // Message d'erreur si la date de facturation est requise
        [Display(Name = "Date de facturation")] // Affiche le nom de la date de facturation dans le formulaire 
        public DateTime DateFacturation { get; set; }

        [Required(ErrorMessage = "Le mode de paiement est requis")] // Message d'erreur si le mode de paiement est requis
        [Display(Name = "Mode de paiement")] // Affiche le nom du mode de paiement dans le formulaire 
        public string ModeDePaiement { get; set; }

        [Required] // Message d'erreur si la commande est requise
        public int CommandeId { get; set; }
        [ForeignKey("CommandeId")] // Clé étrangère pour la relation avec la commande
        [Display(Name = "Commande")] // Affiche le nom de la commande dans le formulaire 
        public Commande Commande { get; set; }

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

        [Display(Name = "Numéro de facture")] // Affiche le nom du numéro de facture dans le formulaire 
        public string NumeroFacture { get; set; }

        [Display(Name = "TVA")] // Affiche le nom de la TVA dans le formulaire 
        [DataType(DataType.Currency)] // Type de données de la TVA
        public decimal TVA { get; set; }

        [Display(Name = "Montant HT")] // Affiche le nom du montant HT dans le formulaire 
        [DataType(DataType.Currency)] // Type de données du montant HT  
        public decimal MontantHT { get; set; }
    }
}
