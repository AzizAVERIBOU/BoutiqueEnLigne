using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoutiqueEnLigne.Models;

namespace BoutiqueEnLigne.Models
{
    public class CartItem
    {
        public Produit Product { get; set; }
        public int Quantity { get; set; }
    }
} 