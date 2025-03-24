namespace BoutiqueEnLigne.Models
{
    public class Vendeur : Utilisateur
    {
        public int NumeroVendeur { get; set; }
        public string NomEntreprise { get; set; }
        public List<Produit> ProduitsVendus { get; set; }
        public List<Client> Clients { get; set; }
        public List<Commande> Commandes { get; set; }
    }
}
