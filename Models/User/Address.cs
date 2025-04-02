using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BoutiqueEnLigne.Models.User
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        [Required(ErrorMessage = "L'adresse est requise")]
        [Display(Name = "Adresse")]
        public string Address1 { get; set; }

        [Required(ErrorMessage = "La ville est requise")]
        [Display(Name = "Ville")]
        public string City { get; set; }

        [Required(ErrorMessage = "Le code postal est requis")]
        [Display(Name = "Code postal")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Le code postal doit contenir 5 chiffres")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "L'état/région est requis")]
        [Display(Name = "État/Région")]
        public string State { get; set; }

        public int? CoordinatesId { get; set; }
        public Coordinates Coordinates { get; set; }

        // Clé étrangère pour l'utilisateur
        public int UserId { get; set; }

        // Propriété de navigation vers l'utilisateur
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
} 