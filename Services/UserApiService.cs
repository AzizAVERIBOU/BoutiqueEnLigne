using System.Text.Json;
using BoutiqueEnLigne.Models;
using BoutiqueEnLigne.Models.User;
using BoutiqueEnLigne.Data;
using Microsoft.EntityFrameworkCore;

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
                var response = await _httpClient.GetStringAsync($"{API_URL}/users");
                Console.WriteLine($"Réponse de l'API : {response}");
                
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                
                var users = JsonSerializer.Deserialize<List<JsonUser>>(response, options);
                Console.WriteLine($"Nombre d'utilisateurs reçus de l'API : {users?.Count ?? 0}");

                if (users != null)
                {
                    foreach (var user in users)
                    {
                        // Vérifier si l'utilisateur existe déjà
                        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                        if (existingUser != null)
                        {
                            Console.WriteLine($"L'utilisateur {user.Email} existe déjà, on le saute.");
                            continue;
                        }

                        var newUser = new Client
                        {
                            Nom = user.Name?.Lastname ?? user.Username ?? "User" + user.Id,
                            Prenom = user.Name?.Firstname ?? "User",
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
                        Console.WriteLine($"Utilisateur ajouté : {newUser.Email} avec le mot de passe : {newUser.MotDePasse}");
                    }

                    var resultat = await _context.SaveChangesAsync();
                    Console.WriteLine($"Nombre d'utilisateurs sauvegardés : {resultat}");

                    // Afficher tous les utilisateurs créés pour vérification
                    var allUsers = await _context.Users.ToListAsync();
                    Console.WriteLine("\nListe des utilisateurs créés :");
                    foreach (var u in allUsers)
                    {
                        Console.WriteLine($"Email: {u.Email}, Mot de passe: {u.MotDePasse}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du peuplement des utilisateurs : {ex.Message}");
                Console.WriteLine($"Stack trace : {ex.StackTrace}");
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
        public JsonName Name { get; set; }
    }

    public class JsonName
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }

    public class JsonAddress // Classe pour désérialiser les données de l'API
    {
        public string Street { get; set; }
        public int Number { get; set; }
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