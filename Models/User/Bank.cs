using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BoutiqueEnLigne.Models.User
{
    public class Bank
    {
        [Key]
        public int BankId { get; set; }

        [JsonPropertyName("cardExpire")]
        [Display(Name = "Date d'expiration")]
        [Required(ErrorMessage = "La date d'expiration est requise")]
        public string CardExpire { get; set; }

        [JsonPropertyName("cardNumber")]
        [Display(Name = "Numéro de carte")]
        [Required(ErrorMessage = "Le numéro de carte est requis")]
        public string CardNumber { get; set; }

        [JsonPropertyName("cardType")]
        [Display(Name = "Type de carte")]
        [Required(ErrorMessage = "Le type de carte est requis")]
        public string CardType { get; set; }

        [JsonPropertyName("currency")]
        [Display(Name = "Devise")]
        [Required(ErrorMessage = "La devise est requise")]
        public string Currency { get; set; }

        [JsonPropertyName("iban")]
        [Display(Name = "IBAN")]
        [Required(ErrorMessage = "L'IBAN est requis")]
        [RegularExpression(@"^[A-Z]{2}[0-9]{2}[A-Z0-9]{11,30}$", ErrorMessage = "Format IBAN invalide")]
        public string Iban { get; set; }

        [JsonPropertyName("bankName")]
        public string BankName { get; set; }
    }
} 