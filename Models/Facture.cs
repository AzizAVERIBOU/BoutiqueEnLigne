namespace BoutiqueEnLigne.Models
{
    public class Facture
    {
        public int FactureId { get; set; }
        public List<Produit> Produits { get; set; }
        public decimal PrixTotal { get; set; }
        public DateTime DateFacturation { get; set; }
        public String ModeDePaiement { get; set; }

    }
}
