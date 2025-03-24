namespace BoutiqueEnLigne.Models
{
    public class Client : Utilisateur
    {
        public int NumeroClient { get; set; }

        public Commande Commande { get; set; }
        public List<Commande> Commandes { get; set; }

        public List<Vendeur> Vendeur { get; set; }
        
        public List<Produit> Produits { get; set; }
        public List<Produit> ProduitsFavoris { get; set; } 
        public List<Produit> ProduitsAchat { get; set; } 
    }
}
