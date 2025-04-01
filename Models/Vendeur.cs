using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BoutiqueEnLigne.Models.User;

namespace BoutiqueEnLigne.Models
{
    public class Vendeur : Models.User.User
    {
        public int NumeroVendeur { get; set; }

        [Required(ErrorMessage = "Le nom de l'entreprise est requis")] // Message d'erreur si le nom de l'entreprise est requis
        [Display(Name = "Nom de l'entreprise")] // Affiche le nom de l'entreprise dans le formulaire 
        [StringLength(100, ErrorMessage = "Le nom de l'entreprise ne peut pas dépasser 100 caractères")] // Message d'erreur si le nom de l'entreprise dépasse 100 caractères
        public string NomEntreprise { get; set; }

        [Display(Name = "Produits vendus")] // Affiche le nom des produits vendus dans le formulaire 
        public List<Product> ProduitsVendus { get; set; }

        [Display(Name = "Relations Client-Vendeur")] // Affiche le nom des relations client-vendeur dans le formulaire 
        public List<ClientVendeur> ClientVendeurs { get; set; }

        [Display(Name = "Commandes")] // Affiche le nom des commandes dans le formulaire 
        public List<Commande> Commandes { get; set; }

        [Display(Name = "SIRET")] // Affiche le nom du numéro SIRET dans le formulaire 
        [RegularExpression(@"^\d{14}$", ErrorMessage = "Le numéro SIRET doit contenir 14 chiffres")] // Message d'erreur si le numéro SIRET n'est pas composé de 14 chiffres
        public string NumeroSIRET { get; set; }

        [Display(Name = "Description de l'entreprise")] // Affiche la description de l'entreprise dans le formulaire 
        [StringLength(500, ErrorMessage = "La description ne peut pas dépasser 500 caractères")] // Message d'erreur si la description dépasse 500 caractères
        public string DescriptionEntreprise { get; set; }

        [Display(Name = "Site web")] // Affiche le nom du site web dans le formulaire 
        [Url(ErrorMessage = "L'URL n'est pas valide")] // Message d'erreur si l'URL n'est pas valide
        public string SiteWeb { get; set; }

        [Display(Name = "Date d'inscription")] // Affiche la date d'inscription dans le formulaire 
        public DateTime DateInscription { get; set; }

        [Display(Name = "Statut")] // Affiche le nom du statut dans le formulaire   
        public bool EstActif { get; set; }
    }
}
