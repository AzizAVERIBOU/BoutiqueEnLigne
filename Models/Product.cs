using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BoutiqueEnLigne.Models
{
    public class Product
    {
        [Key]
        [JsonPropertyName("id")] // Id du produit dans la base de données
        public int ProduitId { get; set; }

        [Required(ErrorMessage = "Le nom est requis")] // Message d'erreur si le nom est requis
        [Display(Name = "Nom")] // Affiche le nom du produit dans le formulaire 
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")] // Message d'erreur si le nom dépasse 100 caractères
        [JsonPropertyName("title")] // Nom du produit dans la base de données
        public string Nom { get; set; }

        [Display(Name = "Description")] // Affiche la description du produit dans le formulaire 
        [StringLength(500, ErrorMessage = "La description ne peut pas dépasser 500 caractères")] // Message d'erreur si la description dépasse 500 caractères
        [JsonPropertyName("description")] // Description du produit dans la base de données
        public string Description { get; set; }

        [Required(ErrorMessage = "Le prix est requis")] // Message d'erreur si le prix est requis
        [Display(Name = "Prix")] // Affiche le prix du produit dans le formulaire 
        [DataType(DataType.Currency)] // Type de données du prix
        [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être positif")] // Message d'erreur si le prix n'est pas positif
        [JsonPropertyName("price")] // Prix du produit dans la base de données
        public decimal Prix { get; set; }

        [Required(ErrorMessage = "La catégorie est requise")] // Message d'erreur si la catégorie est requise
        [Display(Name = "Catégorie")] // Affiche la catégorie du produit dans le formulaire 
        [JsonPropertyName("category")] // Catégorie du produit dans la base de données
        public string Categorie { get; set; }

        [Display(Name = "Image")] // Affiche l'image du produit dans le formulaire 
        [JsonPropertyName("thumbnail")] // Image du produit dans la base de données
        public string Image { get; set; }

        [Required(ErrorMessage = "La quantité est requise")] // Message d'erreur si la quantité est requise
        [Display(Name = "Quantité")] // Affiche la quantité du produit dans le formulaire 
        [Range(0, int.MaxValue, ErrorMessage = "La quantité doit être positive")] // Message d'erreur si la quantité n'est pas positive
        [JsonPropertyName("stock")] // Quantité du produit dans la base de données
        public int Quantite { get; set; }

        [Display(Name = "Disponibilité")] // Affiche la disponibilité du produit dans le formulaire 
        public DisponibiliteProduit Disponibilite { get; set; }

        [Display(Name = "Vendeur")] // Affiche le vendeur du produit dans le formulaire 
        public int VendeurId { get; set; }
        [ForeignKey("VendeurId")] // Clé étrangère pour la relation avec le vendeur
        public Vendeur Vendeur { get; set; }

        [Display(Name = "Date d'ajout")] // Affiche la date d'ajout du produit dans le formulaire 
        public DateTime DateAjout { get; set; }

        [Display(Name = "Date de modification")] // Affiche la date de modification du produit dans le formulaire 
        public DateTime DateModification { get; set; }

        [Display(Name = "Pourcentage de réduction")] // Affiche le pourcentage de réduction du produit dans le formulaire 
        [JsonPropertyName("discountPercentage")] // Pourcentage de réduction du produit dans la base de données
        public decimal PourcentageReduction { get; set; }

        [Display(Name = "Note")] // Affiche la note du produit dans le formulaire 
        [JsonPropertyName("rating")] // Note du produit dans la base de données
        public decimal Note { get; set; }

        [Display(Name = "Marque")] // Affiche la marque du produit dans le formulaire 
        [JsonPropertyName("brand")] // Marque du produit dans la base de données
        public string Marque { get; set; }

        [Display(Name = "Images")] // Affiche les images du produit dans le formulaire 
        [JsonPropertyName("images")] // Images du produit dans la base de données
        public List<string> Images { get; set; }

        // Clés étrangères pour les relations avec Client
        public int? ClientId { get; set; }
        [ForeignKey("ClientId")] // Clé étrangère pour la relation avec le client
        public Client Client { get; set; }

        public int? ClientFavoriId { get; set; }
        [ForeignKey("ClientFavoriId")] // Clé étrangère pour la relation avec le client
        public Client ClientFavori { get; set; }

        public int? ClientAchatId { get; set; }
        [ForeignKey("ClientAchatId")] // Clé étrangère pour la relation avec le client  
        public Client ClientAchat { get; set; }
    }
} 