using System.Text.Json;
using BoutiqueEnLigne.Models;
using BoutiqueEnLigne.Models.User;
using BoutiqueEnLigne.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoutiqueEnLigne.Services
{
    public class AuthApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthApiService> _logger;
        private readonly string _baseUrl = "https://dummyjson.com";

        public AuthApiService(HttpClient httpClient, ILogger<AuthApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

        public async Task<LoginResponse> LoginAsync(string username, string password)
        {
            try
            {
                _logger.LogInformation("Tentative de connexion pour l'utilisateur: {Username}", username);

                var loginRequest = new
                {
                    username = username,
                    password = password
                };

                var response = await _httpClient.PostAsJsonAsync("/auth/login", loginRequest);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var loginResponse = JsonSerializer.Deserialize<LoginResponse>(content, options);
                
                if (loginResponse != null && loginResponse.Token != null)
                {
                    _logger.LogInformation("Connexion réussie pour l'utilisateur: {Username}", username);
                    return loginResponse;
                }

                _logger.LogWarning("Échec de la connexion pour l'utilisateur: {Username}", username);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la tentative de connexion pour l'utilisateur: {Username}", username);
                throw;
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Récupération des informations de l'utilisateur avec l'ID: {Id}", id);

                var response = await _httpClient.GetAsync($"/users/{id}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var apiUser = JsonSerializer.Deserialize<JsonUser>(content, options);
                if (apiUser == null)
                {
                    _logger.LogWarning("Utilisateur non trouvé avec l'ID: {Id}", id);
                    return null;
                }

                _logger.LogInformation("Utilisateur trouvé: {FirstName} {LastName} (ID: {Id})", 
                    apiUser.FirstName, apiUser.LastName, apiUser.Id);

                return new User
                {
                    Id = apiUser.Id,
                    Email = apiUser.Email,
                    Nom = apiUser.LastName,
                    Prenom = apiUser.FirstName,
                    DateInscription = DateTime.Now,
                    Role = RoleUtilisateur.Client
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de l'utilisateur {Id}", id);
                throw;
            }
        }
    }

    public class LoginResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Token { get; set; }
    }

    public class JsonUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
    }
} 