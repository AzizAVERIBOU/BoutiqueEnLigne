namespace BoutiqueEnLigne.Models
{
    public class Produit
    {
        //itre, la description, le prix, la catégorie, l’image, etc
        public int ProduitId { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public decimal Prix { get; set; }
        public string Categorie { get; set; }
        public string Image { get; set; }
        public int Quantite { get; set; }
        //public Enum Disponibilite { get; set; } : { rupture, stock, nombreLimite}

    }
}
