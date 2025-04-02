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

        public UserApiService(
            HttpClient httpClient,
            BoutiqueEnLigneContext context,
            ILogger<UserApiService> logger)
        {
            _httpClient = httpClient;
            _context = context;
            _logger = logger;
            _httpClient.BaseAddress = new Uri("https://dummyjson.com/");
        }

        public async Task<User> AuthenticateUser(string email, string password)
        {
            try
            {
                _logger.LogInformation("Tentative d'authentification pour l'email: {Email}", email);

                var response = await _httpClient.GetAsync("/users");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<DummyUser>>(content);

                if (apiResponse?.Users == null)
                {
                    _logger.LogWarning("Aucun utilisateur trouvé dans l'API");
                    return null;
                }

                var apiUser = apiResponse.Users.FirstOrDefault(u => 
                    u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && 
                    u.Password == password);

                if (apiUser == null)
                {
                    _logger.LogWarning("Utilisateur non trouvé dans l'API");
                    return null;
                }

                _logger.LogInformation("Utilisateur trouvé dans l'API: Email={Email}, FirstName={FirstName}, LastName={LastName}",
                    apiUser.Email, apiUser.FirstName, apiUser.LastName);

                var existingUser = await _context.Users
                    .Include(u => u.Address)
                        .ThenInclude(a => a.Coordinates)
                    .Include(u => u.Bank)
                    .Include(u => u.Company)
                        .ThenInclude(c => c.Address)
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (existingUser != null)
                {
                    existingUser.DerniereConnexion = DateTime.Now;
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Dernière connexion mise à jour pour l'utilisateur: {Email}", email);
                    return existingUser;
                }

                // Créer le nouvel utilisateur d'abord
                var newUser = new Client
                {
                    Email = apiUser.Email,
                    MotDePasse = apiUser.Password,
                    Nom = apiUser.LastName,
                    Prenom = apiUser.FirstName,
                    Role = RoleUtilisateur.Client,
                    DateInscription = DateTime.Now,
                    DerniereConnexion = DateTime.Now,
                    EstActif = true,
                    Image = apiUser.Image
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                // Créer et associer les coordonnées si nécessaire
                Coordinates coordinates = null;
                if (apiUser.Address?.Coordinates != null)
                {
                    coordinates = new Coordinates
                    {
                        Latitude = apiUser.Address.Coordinates.Latitude,
                        Longitude = apiUser.Address.Coordinates.Longitude
                    };
                    _context.Coordinates.Add(coordinates);
                    await _context.SaveChangesAsync();
                }

                // Créer et associer l'adresse
                if (apiUser.Address != null && !string.IsNullOrEmpty(apiUser.Address.Address1))
                {
                    _logger.LogInformation("Création de l'adresse pour l'utilisateur: {Email}, Address1={Address1}, City={City}",
                        email, apiUser.Address.Address1, apiUser.Address.City);

                    var address = new Address
                    {
                        Address1 = apiUser.Address.Address1 ?? "Adresse non spécifiée",
                        City = apiUser.Address.City ?? "Ville non spécifiée",
                        PostalCode = apiUser.Address.PostalCode ?? "00000",
                        State = apiUser.Address.State ?? "État non spécifié",
                        Coordinates = coordinates,
                        UserId = newUser.Id
                    };
                    _context.Addresses.Add(address);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning("Aucune adresse valide trouvée pour l'utilisateur: {Email}", email);
                    
                    // Créer une adresse par défaut
                    var address = new Address
                    {
                        Address1 = "Adresse non spécifiée",
                        City = "Ville non spécifiée",
                        PostalCode = "00000",
                        State = "État non spécifié",
                        UserId = newUser.Id
                    };
                    _context.Addresses.Add(address);
                    await _context.SaveChangesAsync();
                }

                // Créer et associer la banque
                if (apiUser.Bank != null)
                {
                    var bank = new Bank
                    {
                        BankId = newUser.Id,  // Utiliser l'ID de l'utilisateur comme clé étrangère
                        CardExpire = apiUser.Bank.CardExpire,
                        CardNumber = apiUser.Bank.CardNumber,
                        CardType = apiUser.Bank.CardType,
                        Currency = apiUser.Bank.Currency,
                        Iban = apiUser.Bank.Iban,
                        BankName = apiUser.Bank.BankName
                    };
                    _context.Banks.Add(bank);
                    await _context.SaveChangesAsync();
                }

                // Créer et associer l'entreprise
                if (apiUser.Company != null)
                {
                    var company = new Company
                    {
                        CompanyId = newUser.Id,  // Utiliser l'ID de l'utilisateur comme clé étrangère
                        Name = apiUser.Company.Name,
                        Title = apiUser.Company.Title,
                        Department = apiUser.Company.Department
                    };
                    _context.Companies.Add(company);
                    await _context.SaveChangesAsync();
                }

                // Recharger l'utilisateur avec toutes ses relations
                return await _context.Users
                    .Include(u => u.Address)
                        .ThenInclude(a => a.Coordinates)
                    .Include(u => u.Bank)
                    .Include(u => u.Company)
                        .ThenInclude(c => c.Address)
                    .FirstAsync(u => u.Id == newUser.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'authentification de l'utilisateur: {Email}", email);
                throw;
            }
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/users");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<DummyUser>>(content);

                if (apiResponse?.Users == null)
                {
                    return new List<User>();
                }

                var users = new List<User>();
                foreach (var apiUser in apiResponse.Users)
                {
                    // Créer l'adresse
                    Address address = null;
                    if (apiUser.Address != null)
                    {
                        address = new Address
                        {
                            Address1 = apiUser.Address.Address1,
                            City = apiUser.Address.City,
                            PostalCode = apiUser.Address.PostalCode,
                            State = apiUser.Address.State,
                            Coordinates = apiUser.Address.Coordinates
                        };
                        _context.Addresses.Add(address);
                    }

                    // Créer la banque
                    Bank bank = null;
                    if (apiUser.Bank != null)
                    {
                        bank = new Bank
                        {
                            CardExpire = apiUser.Bank.CardExpire,
                            CardNumber = apiUser.Bank.CardNumber,
                            CardType = apiUser.Bank.CardType,
                            Currency = apiUser.Bank.Currency,
                            Iban = apiUser.Bank.Iban
                        };
                        _context.Banks.Add(bank);
                    }

                    // Créer l'entreprise
                    Company company = null;
                    if (apiUser.Company != null)
                    {
                        company = new Company
                        {
                            Name = apiUser.Company.Name,
                            Title = apiUser.Company.Title,
                            Department = apiUser.Company.Department,
                            Address = apiUser.Company.Address
                        };
                        _context.Companies.Add(company);
                    }

                    var user = new Client
                    {
                        Email = apiUser.Email,
                        MotDePasse = apiUser.Password,
                        Nom = apiUser.LastName,
                        Prenom = apiUser.FirstName,
                        Role = RoleUtilisateur.Client,
                        DateInscription = DateTime.Now,
                        DerniereConnexion = DateTime.Now,
                        EstActif = true,
                        Image = apiUser.Image,
                        Address = address,
                        Bank = bank,
                        Company = company
                    };
                    users.Add(user);
                }

                await _context.SaveChangesAsync();
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des utilisateurs depuis l'API");
                throw;
            }
        }

        public async Task<string> GetApiStructure()
        {
            try
            {
                var response = await _httpClient.GetAsync("/users");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<DummyUser>>(content);

                if (apiResponse?.Users == null || !apiResponse.Users.Any())
                {
                    return "Aucun utilisateur trouvé dans l'API";
                }

                var firstUser = apiResponse.Users.First();
                var structure = new System.Text.StringBuilder();
                structure.AppendLine("Structure des données utilisateur de l'API :");
                structure.AppendLine($"ID: {firstUser.Id}");
                structure.AppendLine($"Username: {firstUser.Username}");
                structure.AppendLine($"Email: {firstUser.Email}");
                structure.AppendLine($"FirstName: {firstUser.FirstName}");
                structure.AppendLine($"LastName: {firstUser.LastName}");
                structure.AppendLine($"Gender: {firstUser.Gender}");
                structure.AppendLine($"Phone: {firstUser.Phone}");
                structure.AppendLine($"Address: {firstUser.Address?.Address1}, {firstUser.Address?.City}");
                structure.AppendLine($"Bank: {firstUser.Bank?.BankName}");
                structure.AppendLine($"Company: {firstUser.Company?.Name}");

                return structure.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de la structure de l'API");
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