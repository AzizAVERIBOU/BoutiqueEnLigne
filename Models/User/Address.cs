using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BoutiqueEnLigne.Models.User
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        [JsonPropertyName("address")] // Nom de l'adresse dans la base de données   
        [Display(Name = "Adresse")] // Affiche le nom de l'adresse dans le formulaire 
        [Required(ErrorMessage = "L'adresse est requise")]
        public string Street { get; set; }

        [JsonPropertyName("city")] // Nom de la ville dans la base de données
        [Display(Name = "Ville")]
        [Required(ErrorMessage = "La ville est requise")]
        public string City { get; set; }

        [JsonPropertyName("coordinates")] // Coordonnées de la ville dans la base de données
        [Display(Name = "Coordonnées")]
        public Coordinates Coordinates { get; set; }

        public int? CoordinatesId { get; set; }

        [JsonPropertyName("postalCode")] // Code postal dans la base de données
        [Display(Name = "Code postal")]
        [Required(ErrorMessage = "Le code postal est requis")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Le code postal doit contenir 5 chiffres")]
        public string PostalCode { get; set; }

        [JsonPropertyName("state")] // État/Région dans la base de données
        [Display(Name = "État/Région")]
        public string State { get; set; }
    }
} 