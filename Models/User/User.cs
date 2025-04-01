using System.ComponentModel.DataAnnotations;

namespace BoutiqueEnLigne.Models.User
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le prénom est requis")]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Le nom est requis")]
        [Display(Name = "Nom")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [Display(Name = "Mot de passe")]
        public string MotDePasse { get; set; }

        [Display(Name = "Image de profil")]
        public string Image { get; set; } = "~/images/membre1.jpg";
    }
} 