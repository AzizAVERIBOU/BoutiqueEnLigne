using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BoutiqueEnLigne.Models.User
{
    public class Bank
    {
        [Key]
        public int BankId { get; set; }

        [JsonPropertyName("cardExpire")] // Date d'expiration dans la base de données
        [Display(Name = "Date d'expiration")] // Affiche le nom de la date d'expiration dans le formulaire 
        [RegularExpression(@"^(0[1-9]|1[0-2])\/([0-9]{2})$", ErrorMessage = "Format invalide. Utilisez MM/YY")] // Format de la date d'expiration
        public string CardExpire { get; set; }

        [JsonPropertyName("cardNumber")] // Numéro de carte dans la base de données
        [Display(Name = "Numéro de carte")] // Affiche le nom du numéro de carte dans le formulaire 
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Le numéro de carte doit contenir 16 chiffres")] // Format du numéro de carte
        public string CardNumber { get; set; }

        [JsonPropertyName("cardType")] // Type de carte dans la base de données
        [Display(Name = "Type de carte")] // Affiche le nom du type de carte dans le formulaire 
        public string CardType { get; set; }

        [JsonPropertyName("currency")] // Devise dans la base de données
        [Display(Name = "Devise")] // Affiche le nom de la devise dans le formulaire 
        public string Currency { get; set; }

        [JsonPropertyName("iban")] // IBAN dans la base de données  
        [Display(Name = "IBAN")] // Affiche le nom de l'IBAN dans le formulaire 
        [RegularExpression(@"^[A-Z]{2}[0-9]{2}[A-Z0-9]{11,30}$", ErrorMessage = "Format IBAN invalide")] // Format de l'IBAN
        public string Iban { get; set; }
    }
} 