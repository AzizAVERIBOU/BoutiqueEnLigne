namespace BoutiqueEnLigne.Models
{
    public enum StatutCommande // Enumération des statuts de commande
    {
        EnCours,
        Livree,
        Annulee
    }

    public enum ValidationPaiement // Enumération des validations de paiement
    {
        Payer,
        NonPayer
    }

    public enum DisponibiliteProduit // Enumération des disponibilités de produit   
    {
        Rupture,
        Stock,
        NombreLimite
    }
} 