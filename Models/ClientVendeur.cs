using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoutiqueEnLigne.Models
{
    public class ClientVendeur // Relation entre un client et un vendeur
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public int VendeurId { get; set; }
        public Vendeur Vendeur { get; set; }
    }
} 