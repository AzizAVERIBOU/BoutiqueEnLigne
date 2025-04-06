using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BoutiqueEnLigne.Models
{
    public class Produit
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom du produit est requis")]
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "La description est requise")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Le prix est requis")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être supérieur à 0")]
        public decimal Prix { get; set; }

        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "La catégorie est requise")]
        public string Categorie { get; set; }

        public int Stock { get; set; }
    }
} 