using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BoutiqueEnLigne.Models.User;

namespace BoutiqueEnLigne.Models
{
    public class Client : Models.User.User
    {
        public int NumeroClient { get; set; }

        [Display(Name = "Commandes")] // Affiche le nom des commandes dans le formulaire 
        public List<Commande> Commandes { get; set; }

        [Display(Name = "Relations Client-Vendeur")] // Affiche le nom des relations client-vendeur dans le formulaire 
        public List<ClientVendeur> ClientVendeurs { get; set; }

        [Display(Name = "Produits")] // Affiche le nom des produits dans le formulaire 
        [InverseProperty("Client")] // Inverse la propriété Client pour la relation Produits
        public List<Product> Produits { get; set; }

        [Display(Name = "Produits favoris")] // Affiche le nom des produits favoris dans le formulaire 
        [InverseProperty("ClientFavori")] // Inverse la propriété ClientFavori pour la relation ProduitsFavoris
        public List<Product> ProduitsFavoris { get; set; }

        [Display(Name = "Produits achetés")] // Affiche le nom des produits achetés dans le formulaire 
        [InverseProperty("ClientAchat")] // Inverse la propriété ClientAchat pour la relation ProduitsAchat
        public List<Product> ProduitsAchat { get; set; }

        [Display(Name = "Panier")] // Affiche le nom du panier dans le formulaire 
        public Panier Panier { get; set; }

        [Display(Name = "Date d'inscription")] // Affiche le nom de la date d'inscription dans le formulaire 
        public DateTime DateInscription { get; set; }

        [Display(Name = "Dernière connexion")] // Affiche le nom de la dernière connexion dans le formulaire 
        public DateTime DerniereConnexion { get; set; }

        [Display(Name = "Statut")] // Affiche le nom du statut dans le formulaire 
        public bool EstActif { get; set; }

        [Display(Name = "Préférences de notification")] // Affiche le nom des préférences de notification dans le formulaire 
        public bool NotificationsEmail { get; set; }

        [Display(Name = "Newsletter")] // Affiche le nom de la newsletter dans le formulaire    
        public bool InscritNewsletter { get; set; }
    }
}
