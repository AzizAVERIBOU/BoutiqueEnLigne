using System.Text.Json;
using BoutiqueEnLigne.Models;
using BoutiqueEnLigne.Models.User;
using BoutiqueEnLigne.Data;
using Microsoft.EntityFrameworkCore;

namespace BoutiqueEnLigne.Services
{
    public class ProductApiService
    {
        private readonly HttpClient _httpClient;
        private readonly BoutiqueEnLigneContext _context;
        private const string API_URL = "https://dummyjson.com";

        public ProductApiService(HttpClient httpClient, BoutiqueEnLigneContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task PopulateProductsFromApi()
        {
            try
            {
                // Get all users to use as vendors
                var users = await _context.Users.ToListAsync();
                if (!users.Any())
                {
                    Console.WriteLine("No users found in the database. Please ensure users are populated first.");
                    return;
                }

                // Get all products without limit
                var response = await _httpClient.GetStringAsync($"{API_URL}/products");
                Console.WriteLine($"Réponse de l'API : {response}");
                
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                
                var result = JsonSerializer.Deserialize<JsonProductResponse>(response, options);
                Console.WriteLine($"Désérialisation réussie : {result != null}");
                if (result != null)
                {
                    Console.WriteLine($"Total : {result.Total}, Skip : {result.Skip}, Limit : {result.Limit}");
                }

                if (result?.Products != null)
                {
                    Console.WriteLine($"Nombre de produits reçus de l'API : {result.Products.Count}");
                    
                    // Create a random number generator for vendor assignment
                    var random = new Random();
                    
                    foreach (var product in result.Products)
                    {
                        // Randomly select a user as the vendor
                        var randomVendor = users[random.Next(0, users.Count)];
                        
                        var newProduct = new Product
                        {
                            Nom = product.Title,
                            Description = product.Description,
                            Prix = product.Price,
                            Image = product.Thumbnail,
                            Categorie = product.Category,
                            Note = product.Rating,
                            Quantite = product.Stock,
                            PourcentageReduction = product.DiscountPercentage,
                            Marque = string.IsNullOrEmpty(product.Brand) ? "Generic" : product.Brand,
                            Images = product.Images,
                            DateAjout = DateTime.Now,
                            DateModification = DateTime.Now,
                            Disponibilite = DisponibiliteProduit.Stock,
                            VendeurId = randomVendor.Id // Assign the selected user as vendor
                        };

                        _context.Products.Add(newProduct);
                        Console.WriteLine($"Produit ajouté : {newProduct.Nom} (Vendeur: {randomVendor.Email})");
                    }

                    var resultat = await _context.SaveChangesAsync();
                    Console.WriteLine($"Nombre de produits sauvegardés : {resultat}");
                }
                else
                {
                    Console.WriteLine("Aucun produit reçu de l'API");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du peuplement des produits : {ex.Message}");
                Console.WriteLine($"Stack trace : {ex.StackTrace}");
                throw;
            }
        }
    }

    public class JsonProductResponse
    {
        public List<JsonProduct> Products { get; set; }
        public int Total { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
    }

    public class JsonProduct
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal Rating { get; set; }
        public int Stock { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public string Thumbnail { get; set; }
        public List<string> Images { get; set; }
    }
} 