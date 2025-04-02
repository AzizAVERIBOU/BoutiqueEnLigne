using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BoutiqueEnLigne.Models.User
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        [JsonPropertyName("street")]
        [Display(Name = "Adresse")]
        [Required(ErrorMessage = "L'adresse est requise")]
        public string Address1 { get; set; }

        [JsonPropertyName("city")]
        [Display(Name = "Ville")]
        [Required(ErrorMessage = "La ville est requise")]
        public string City { get; set; }

        [JsonPropertyName("coordinates")]
        [Display(Name = "Coordonnées")]
        public Coordinates Coordinates { get; set; }

        public int? CoordinatesId { get; set; }

        [JsonPropertyName("postalCode")]
        [Display(Name = "Code postal")]
        [Required(ErrorMessage = "Le code postal est requis")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Le code postal doit contenir 5 chiffres")]
        public string PostalCode { get; set; }

        [JsonPropertyName("state")]
        [Display(Name = "État/Région")]
        [Required(ErrorMessage = "L'état/région est requis")]
        public string State { get; set; }

        // Clé étrangère pour l'utilisateur
        public int UserId { get; set; }

        // Propriété de navigation vers l'utilisateur
        [JsonIgnore]
        public User User { get; set; }
    }
} 