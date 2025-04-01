using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BoutiqueEnLigne.Models.User
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        [JsonPropertyName("address")] // Adresse dans la base de données
        [Display(Name = "Adresse")] // Affiche le nom de l'adresse dans le formulaire 
        public Address Address { get; set; }

        [JsonPropertyName("department")] // Département dans la base de données
        [Display(Name = "Département")] // Affiche le nom du département dans le formulaire 
        public string Department { get; set; }

        [JsonPropertyName("name")] // Nom dans la base de données
        [Display(Name = "Nom")] // Affiche le nom dans le formulaire 
        [Required(ErrorMessage = "Le nom de l'entreprise est requis")] // Message d'erreur si le nom est requis
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")] // Message d'erreur si le nom dépasse 100 caractères
        public string Name { get; set; }

        [JsonPropertyName("title")] // Titre dans la base de données
        [Display(Name = "Titre")] // Affiche le titre dans le formulaire    
        public string Title { get; set; }
    }
} 