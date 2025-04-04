using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BoutiqueEnLigne.Models.User
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le nom est requis")]
        [Display(Name = "Nom")]
        [StringLength(50, ErrorMessage = "Le nom ne peut pas dépasser 50 caractères")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prénom est requis")]
        [Display(Name = "Prénom")]
        [StringLength(50, ErrorMessage = "Le prénom ne peut pas dépasser 50 caractères")]
        public string Prenom { get; set; }

        [Display(Name = "Nom d'utilisateur")]
        [StringLength(50, ErrorMessage = "Le nom d'utilisateur ne peut pas dépasser 50 caractères")]
        public string Username { get; set; }

        [Display(Name = "Genre")]
        public string Genre { get; set; }

        [Display(Name = "Téléphone")]
        [Phone(ErrorMessage = "Format de téléphone invalide")]
        public string Telephone { get; set; }

        [Display(Name = "Date de naissance")]
        [DataType(DataType.Date)]
        public DateTime? DateNaissance { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [Display(Name = "Mot de passe")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir entre 6 et 100 caractères")]
        public string MotDePasse { get; set; }

        [Display(Name = "Date d'inscription")]
        public DateTime DateInscription { get; set; }

        [Display(Name = "Dernière connexion")]
        public DateTime DerniereConnexion { get; set; }

        [Display(Name = "Rôle")]
        public RoleUtilisateur Role { get; set; }

        [Display(Name = "Est actif")]
        public bool EstActif { get; set; }

        [Display(Name = "Notifications par email")]
        public bool NotificationsEmail { get; set; }

        [Display(Name = "Inscrit à la newsletter")]
        public bool InscritNewsletter { get; set; }

        [Display(Name = "Image de profil")]
        public string Image { get; set; }

        [Display(Name = "Site web")]
        public string? SiteWeb { get; set; }

        // Propriétés de navigation
        public virtual Address Address { get; set; }
        public virtual Bank Bank { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<Commande> Commandes { get; set; }
    }

    public enum RoleUtilisateur
    {
        Client,
        Vendeur,
        Admin
    }
} 