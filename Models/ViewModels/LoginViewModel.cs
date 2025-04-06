using System.ComponentModel.DataAnnotations;

namespace BoutiqueEnLigne.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "L'adresse email est requise")]
        [EmailAddress(ErrorMessage = "L'adresse email n'est pas valide")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Se souvenir de moi")]
        public bool RememberMe { get; set; }
    }
} 