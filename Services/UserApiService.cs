using System.Text.Json;
using BoutiqueEnLigne.Models.User;
using BoutiqueEnLigne.Data;
using BoutiqueEnLigne.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoutiqueEnLigne.Services
{
    public class UserApiService
    {
        private readonly HttpClient _httpClient;
        private readonly BoutiqueEnLigneContext _context;
        private readonly ILogger<UserApiService> _logger;
        private const string API_URL = "https://fakestoreapi.com";

        public UserApiService(HttpClient httpClient, BoutiqueEnLigneContext context, ILogger<UserApiService> logger)
        {
            _httpClient = httpClient;
            _context = context;
            _logger = logger;
            _httpClient.BaseAddress = new Uri(API_URL);
        }

        public async Task<User> AuthenticateUser(string email, string password)
        {
            try
            {
                _logger.LogInformation($"Tentative d'authentification pour l'email: {email}");
                
                var response = await _httpClient.GetAsync("/users");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var apiUsers = JsonSerializer.Deserialize<List<ApiUser>>(content, options);
                if (apiUsers == null || !apiUsers.Any())
                {
                    _logger.LogWarning("Aucun utilisateur trouvé dans l'API");
                    return null;
                }

                var apiUser = apiUsers.FirstOrDefault(u => 
                    u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && 
                    u.Password == password);

                if (apiUser == null)
                {
                    _logger.LogWarning($"Aucun utilisateur trouvé avec l'email: {email}");
                    return null;
                }

                _logger.LogInformation($"Utilisateur trouvé dans l'API: Email={apiUser.Email}, FirstName={apiUser.FirstName}, LastName={apiUser.LastName}");

                // Vérifier si l'utilisateur existe déjà dans la base de données locale
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (existingUser == null)
                {
                    // Créer un nouvel utilisateur dans la base de données locale
                    var newUser = new Client
                    {
                        Email = apiUser.Email,
                        Nom = string.IsNullOrEmpty(apiUser.LastName) ? "Utilisateur" : apiUser.LastName,
                        Prenom = string.IsNullOrEmpty(apiUser.FirstName) ? "John" : apiUser.FirstName,
                        MotDePasse = apiUser.Password,
                        DateInscription = DateTime.Now,
                        DerniereConnexion = DateTime.Now,
                        NumeroClient = apiUser.Id,
                        EstActif = true,
                        NotificationsEmail = true,
                        InscritNewsletter = true,
                        Image = "https://via.placeholder.com/150",
                        Role = RoleUtilisateur.Client
                    };

                    _logger.LogInformation($"Tentative de création d'un nouvel utilisateur: Email={newUser.Email}, Nom={newUser.Nom}, Prenom={newUser.Prenom}");

                    _context.Users.Add(newUser);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Nouvel utilisateur créé dans la base de données locale: {newUser.Email}");
                    return newUser;
                }
                else
                {
                    // Mettre à jour la dernière connexion
                    existingUser.DerniereConnexion = DateTime.Now;
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Dernière connexion mise à jour pour l'utilisateur: {existingUser.Email}");
                    return existingUser;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'authentification");
                throw;
            }
        }
    }

    public class ApiUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
    }
} 