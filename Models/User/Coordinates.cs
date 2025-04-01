using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BoutiqueEnLigne.Models.User
{
    public class Coordinates
    {
        [Key]
        public int CoordinatesId { get; set; }

        [JsonPropertyName("lat")] // Latitude dans la base de données
        [Display(Name = "Latitude")] // Affiche le nom de la latitude dans le formulaire 
        [Range(-90, 90, ErrorMessage = "La latitude doit être comprise entre -90 et 90")] // Message d'erreur si la latitude n'est pas comprise entre -90 et 90
        public double Latitude { get; set; }

        [JsonPropertyName("lng")] // Longitude dans la base de données  
        [Display(Name = "Longitude")] // Affiche le nom de la longitude dans le formulaire 
        [Range(-180, 180, ErrorMessage = "La longitude doit être comprise entre -180 et 180")] // Message d'erreur si la longitude n'est pas comprise entre -180 et 180
        public double Longitude { get; set; }
    }
} 