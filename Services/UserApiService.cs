using System.Text.Json;
using BoutiqueEnLigne.Models;
using BoutiqueEnLigne.Models.User;
using BoutiqueEnLigne.Data;

namespace BoutiqueEnLigne.Services
{
    public class UserApiService // Classe pour récupérer les données de l'API   
    {
        private readonly HttpClient _httpClient; // HttpClient pour récupérer les données de l'API
        private readonly BoutiqueEnLigneContext _context; // Context pour accéder à la base de données
        private const string API_URL = "https://fakestoreapi.com"; // URL de l'API

        public UserApiService(HttpClient httpClient, BoutiqueEnLigneContext context)
        {
            _httpClient = httpClient; // HttpClient pour récupérer les données de l'API
            _context = context; // Context pour accéder à la base de données
        }

        public async Task PopulateUsersFromApi() // Méthode pour récupérer les données de l'API et les insérer dans la base de données
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"{API_URL}/users"); // Récupérer les données de l'API
                var users = JsonSerializer.Deserialize<List<JsonUser>>(response); // Désérialiser les données de l'API

                if (users != null)
                {
                    foreach (var user in users)
                    {
                        var newUser = new Client
                        {
                            Nom = user.Username ?? "User" + user.Id,
                            Prenom = "User", // Valeur par défaut car l'API ne fournit pas de prénom
                            Email = user.Email ?? $"user{user.Id}@example.com",
                            MotDePasse = user.Password ?? $"Password{user.Id}!", // Mot de passe par défaut si null
                            DateInscription = DateTime.Now,
                            DerniereConnexion = DateTime.Now,
                            NumeroClient = user.Id,
                            EstActif = true,
                            NotificationsEmail = true,
                            InscritNewsletter = true,
                            Image = "https://via.placeholder.com/150" // Image par défaut
                        };

                        _context.Users.Add(newUser);
                    }

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du peuplement des utilisateurs : {ex.Message}");
                throw;
            }
        }
    }

    // Classe pour désérialiser les données de l'API
    public class JsonUser // Classe pour désérialiser les données de l'API
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public JsonAddress Address { get; set; }
        public string Phone { get; set; }
    }

    public class JsonAddress // Classe pour désérialiser les données de l'API
    {
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public JsonGeo Geolocation { get; set; }
    }

    public class JsonGeo // Classe pour désérialiser les données de l'API
    {
        public string Lat { get; set; }
        public string Long { get; set; }
    }
} 