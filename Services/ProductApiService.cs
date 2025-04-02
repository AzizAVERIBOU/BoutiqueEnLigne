using System.Text.Json;
using BoutiqueEnLigne.Models;
using Microsoft.EntityFrameworkCore;

namespace BoutiqueEnLigne.Services
{
    public class ProductApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductApiService> _logger;
        private readonly string _baseUrl = "https://dummyjson.com";

        public ProductApiService(HttpClient httpClient, ILogger<ProductApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

        public async Task<List<Product>> GetProductsAsync(int skip = 0, int limit = 24)
        {
            try
            {
                _logger.LogInformation($"Récupération des produits depuis l'API (skip={skip}, limit={limit})");
                
                var response = await _httpClient.GetAsync($"/products?skip={skip}&limit={limit}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var apiResponse = JsonSerializer.Deserialize<JsonProductResponse>(content, options);
                if (apiResponse?.Products == null || !apiResponse.Products.Any())
                {
                    _logger.LogWarning("Aucun produit reçu de l'API");
                    return new List<Product>();
                }

                _logger.LogInformation($"Nombre de produits reçus de l'API : {apiResponse.Products.Count}");

                var products = apiResponse.Products.Select(apiProduct => new Product
                {
                    ProduitId = apiProduct.Id,
                    Nom = apiProduct.Title,
                    Description = apiProduct.Description,
                    Prix = apiProduct.Price,
                    Categorie = apiProduct.Category,
                    Image = apiProduct.Thumbnail,
                    Quantite = apiProduct.Stock,
                    Disponibilite = DisponibiliteProduit.Stock,
                    VendeurId = 1,
                    DateAjout = DateTime.Now,
                    DateModification = DateTime.Now,
                    PourcentageReduction = apiProduct.DiscountPercentage,
                    Note = apiProduct.Rating,
                    Marque = apiProduct.Brand ?? "Marque inconnue",
                    Images = apiProduct.Images ?? new List<string>()
                }).ToList();

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits depuis l'API");
                throw;
            }
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Récupération du produit avec l'ID: {id} depuis l'API");
                
                var response = await _httpClient.GetAsync($"/products/{id}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var apiProduct = JsonSerializer.Deserialize<JsonProduct>(content, options);
                if (apiProduct == null)
                {
                    _logger.LogWarning($"Produit non trouvé avec l'ID: {id}");
                    return null;
                }

                _logger.LogInformation($"Produit trouvé: {apiProduct.Title} (ID: {apiProduct.Id})");

                return new Product
                {
                    ProduitId = apiProduct.Id,
                    Nom = apiProduct.Title,
                    Description = apiProduct.Description,
                    Prix = apiProduct.Price,
                    Categorie = apiProduct.Category,
                    Image = apiProduct.Thumbnail,
                    Quantite = apiProduct.Stock,
                    Disponibilite = DisponibiliteProduit.Stock,
                    VendeurId = 1,
                    DateAjout = DateTime.Now,
                    DateModification = DateTime.Now,
                    PourcentageReduction = apiProduct.DiscountPercentage,
                    Note = apiProduct.Rating,
                    Marque = apiProduct.Brand ?? "Marque inconnue",
                    Images = apiProduct.Images ?? new List<string>()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la récupération du produit {id} depuis l'API");
                throw;
            }
        }

        public async Task<int> GetTotalProductsCountAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/products");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var apiResponse = JsonSerializer.Deserialize<JsonProductResponse>(content, options);
                return apiResponse?.Total ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération du nombre total de produits");
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