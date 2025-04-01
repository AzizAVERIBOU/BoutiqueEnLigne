using System.Text.Json;
using BoutiqueEnLigne.Models;
using BoutiqueEnLigne.Models.User;
using BoutiqueEnLigne.Data;

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
                var response = await _httpClient.GetStringAsync($"{API_URL}/products");
                var result = JsonSerializer.Deserialize<JsonProductResponse>(response);

                if (result?.Products != null)
                {
                    foreach (var product in result.Products)
                    {
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
                            Marque = product.Brand,
                            Images = product.Images,
                            DateAjout = DateTime.Now,
                            DateModification = DateTime.Now,
                            Disponibilite = DisponibiliteProduit.Stock
                        };

                        _context.Products.Add(newProduct);
                    }

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du peuplement des produits : {ex.Message}");
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