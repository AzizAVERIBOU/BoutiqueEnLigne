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

        private async Task<List<DummyUser>> GetUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/users");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<DummyUser>>(content);

                if (apiResponse?.Users == null)
                {
                    Console.WriteLine("Aucun utilisateur trouvé dans l'API");
                    return new List<DummyUser>();
                }

                Console.WriteLine($"Nombre d'utilisateurs reçus de l'API : {apiResponse.Users.Count}");
                return apiResponse.Users;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des utilisateurs: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<User> AuthenticateUser(string email, string password)
        {
            try
            {
                Console.WriteLine($"Tentative d'authentification pour l'email: {email}");
                var users = await GetUsersAsync();
                var user = users.FirstOrDefault(u => u.Email == email);

                if (user != null)
                {
                    Console.WriteLine("\n=== DONNÉES DE L'UTILISATEUR TROUVÉ DANS L'API ===");
                    Console.WriteLine($"ID: {user.Id}");
                    Console.WriteLine($"Email: {user.Email}");
                    Console.WriteLine($"Prénom: {user.FirstName}");
                    Console.WriteLine($"Nom: {user.LastName}");
                    Console.WriteLine($"Genre: {user.Gender}");
                    Console.WriteLine($"Téléphone: {user.Phone}");
                    Console.WriteLine($"Image: {user.Image}");
                    Console.WriteLine($"Date de naissance: {user.BirthDate}");
                    Console.WriteLine($"Nom d'utilisateur: {user.Username}");

                    if (user.Address != null)
                    {
                        Console.WriteLine("\n=== ADRESSE ===");
                        Console.WriteLine($"Adresse: {user.Address.Address1 ?? "Non spécifiée"}");
                        Console.WriteLine($"Ville: {user.Address.City ?? "Non spécifiée"}");
                        Console.WriteLine($"Code postal: {user.Address.PostalCode ?? "Non spécifié"}");
                        Console.WriteLine($"État: {user.Address.State ?? "Non spécifié"}");
                        if (user.Address.Coordinates != null)
                        {
                            Console.WriteLine($"Coordonnées: {user.Address.Coordinates.Latitude}, {user.Address.Coordinates.Longitude}");
                        }
                    }

                    if (user.Bank != null)
                    {
                        Console.WriteLine("\n=== INFORMATIONS BANCAIRES ===");
                        Console.WriteLine($"Carte: {user.Bank.CardNumber}");
                        Console.WriteLine($"Type: {user.Bank.CardType}");
                        Console.WriteLine($"Expiration: {user.Bank.CardExpire}");
                        Console.WriteLine($"IBAN: {user.Bank.Iban}");
                    }

                    if (user.Company != null)
                    {
                        Console.WriteLine("\n=== ENTREPRISE ===");
                        Console.WriteLine($"Nom: {user.Company.Name}");
                        Console.WriteLine($"Département: {user.Company.Department}");
                        Console.WriteLine($"Titre: {user.Company.Title}");
                        Console.WriteLine($"Adresse: {user.Company.Address?.Address1 ?? "Non spécifiée"}");
                    }

                    Console.WriteLine("\n=== CRÉATION DE L'UTILISATEUR DANS LA BASE LOCALE ===");
                    Console.WriteLine("Données de l'utilisateur à créer:");
                    Console.WriteLine($"- Email: {user.Email}");
                    Console.WriteLine($"- Nom: {user.LastName}");
                    Console.WriteLine($"- Prénom: {user.FirstName}");
                    Console.WriteLine($"- Image: {user.Image ?? "default-avatar.png"}");
                    Console.WriteLine($"- Role: Client");

                    var newUser = new User
                    {
                        Email = user.Email,
                        MotDePasse = password,
                        Nom = user.LastName,
                        Prenom = user.FirstName,
                        DateInscription = DateTime.Now,
                        DerniereConnexion = DateTime.Now,
                        EstActif = true,
                        InscritNewsletter = false,
                        NotificationsEmail = false,
                        Image = user.Image ?? "default-avatar.png",
                        Role = RoleUtilisateur.Client
                    };

                    Console.WriteLine("Ajout de l'utilisateur au contexte...");
                    _context.Users.Add(newUser);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Nouvel utilisateur créé dans la base locale avec succès");

                    if (user.Address != null)
                    {
                        Console.WriteLine("\n=== CRÉATION DE L'ADRESSE ===");
                        Console.WriteLine("Données de l'adresse à créer:");
                        Console.WriteLine($"- Address1: {user.Address.Address1 ?? "Non spécifiée"}");
                        Console.WriteLine($"- City: {user.Address.City ?? "Non spécifiée"}");
                        Console.WriteLine($"- PostalCode: {user.Address.PostalCode ?? "00000"}");
                        Console.WriteLine($"- State: {user.Address.State ?? "Non spécifiée"}");
                        Console.WriteLine($"- UserId: {newUser.Id}");

                        var address = new Address
                        {
                            Address1 = user.Address.Address1 ?? "Non spécifiée",
                            City = user.Address.City ?? "Non spécifiée",
                            PostalCode = user.Address.PostalCode ?? "00000",
                            State = user.Address.State ?? "Non spécifiée",
                            UserId = newUser.Id
                        };

                        Console.WriteLine("Ajout de l'adresse au contexte...");
                        _context.Addresses.Add(address);
                        await _context.SaveChangesAsync();
                        Console.WriteLine("Adresse créée avec succès");
                    }
                    else
                    {
                        Console.WriteLine("\n=== CRÉATION DE L'ADRESSE PAR DÉFAUT ===");
                        var defaultAddress = new Address
                        {
                            Address1 = "Non spécifiée",
                            City = "Non spécifiée",
                            PostalCode = "00000",
                            State = "Non spécifiée",
                            UserId = newUser.Id
                        };

                        Console.WriteLine("Ajout de l'adresse par défaut au contexte...");
                        _context.Addresses.Add(defaultAddress);
                        await _context.SaveChangesAsync();
                        Console.WriteLine("Adresse par défaut créée avec succès");
                    }

                    Console.WriteLine("\n=== FIN DE LA CRÉATION DE L'UTILISATEUR ===");
                    return newUser;
                }

                Console.WriteLine("\nUtilisateur non trouvé dans l'API");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErreur lors de l'authentification de l'utilisateur: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    Console.WriteLine($"Inner stack trace: {ex.InnerException.StackTrace}");
                }
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