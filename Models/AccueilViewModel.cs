using System.Collections.Generic;

namespace BoutiqueEnLigne.Models
{
    public class AccueilViewModel
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int ProductsPerPage { get; set; }
        public int TotalProducts { get; set; }
    }
} 