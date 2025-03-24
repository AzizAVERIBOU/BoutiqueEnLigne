using System.ComponentModel.DataAnnotations;

namespace BoutiqueEnLigne.Models
{
    public class Utilisateur
    {
        //nom, prénom, adresse, code postal, carte de crédit, mot de passe
        [Key]
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string MotDePasse { get; set; }
        public string Adresse { get; set; }
        public string CodePostal { get; set; }
        public CarteDeCredit CarteDeCredit { get; set; }

    }
}
