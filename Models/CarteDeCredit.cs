namespace BoutiqueEnLigne.Models
{
    public class CarteDeCredit
    {
        //nom du détenteur, numéro de la carte, date d’expiration, et CCV
        public int NumeroCarte { get; set; }
        public string Nom { get; set; }
        public string Numero { get; set; }
        public string DateExpiration { get; set; }
        public string CCV { get; set; }
        public string CodePostal { get; set; }
    }
}
