using System.ComponentModel.DataAnnotations;

namespace BoutiqueEnLigne.Models
{
    public class CarteDeCredit
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le numéro de carte est requis")] // Message d'erreur si le numéro de carte est requis
        [Display(Name = "Numéro de carte")] // Affiche le nom du numéro de carte dans le formulaire 
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Le numéro de carte doit contenir 16 chiffres")] // Message d'erreur si le numéro de carte n'est pas composé de 16 chiffres
        public string Numero { get; set; }

        [Required(ErrorMessage = "La date d'expiration est requise")] // Message d'erreur si la date d'expiration est requise
        [Display(Name = "Date d'expiration")] // Affiche le nom de la date d'expiration dans le formulaire 
        [RegularExpression(@"^(0[1-9]|1[0-2])\/([0-9]{2})$", ErrorMessage = "Format invalide. Utilisez MM/YY")] // Message d'erreur si la date d'expiration n'est pas composée de 2 chiffres pour le mois et 2 chiffres pour l'année
        public string DateExpiration { get; set; }

        [Required(ErrorMessage = "Le code de sécurité est requis")] // Message d'erreur si le code de sécurité est requis
        [Display(Name = "Code de sécurité")] // Affiche le nom du code de sécurité dans le formulaire 
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "Le code de sécurité doit contenir 3 ou 4 chiffres")] // Message d'erreur si le code de sécurité n'est pas composé de 3 ou 4 chiffres
        public string CodeSecurite { get; set; }

        [Required(ErrorMessage = "Le nom du titulaire est requis")] // Message d'erreur si le nom du titulaire est requis
        [Display(Name = "Nom du titulaire")] // Affiche le nom du titulaire dans le formulaire  
        public string Titulaire { get; set; } 

    }
}
