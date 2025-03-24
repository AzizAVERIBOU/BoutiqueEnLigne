namespace BoutiqueEnLigne.Models
{
    public class Panier
    {
        public int PanierId { get; set; }

        public List<Produit> Produits { get; set; }

        public int Quantite { get; set; }

        public decimal PrixTotal { get; set; }

        public DateTime DateCreation { get; set; }
         
        public DateTime DateModification { get; set; }
        
    }
}
