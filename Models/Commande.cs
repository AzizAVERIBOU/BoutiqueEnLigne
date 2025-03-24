namespace BoutiqueEnLigne.Models
{
    public class Commande
    {
        public int CommandeId { get; set; }
        
        public DateTime DateCommande { get; set; }

        public decimal PrixTotal { get; set; }

        //public Enum Statut { get; set; }  { enCours; livree, annulee}

        //public Enum Validation { get; set; } : { Payer , NonPayer} 
        
    }
}
