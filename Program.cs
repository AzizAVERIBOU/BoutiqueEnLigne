using Microsoft.EntityFrameworkCore;
using BoutiqueEnLigne.Models;
using BoutiqueEnLigne.Models.User;
using BoutiqueEnLigne.Data;
using BoutiqueEnLigne.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Stripe;           

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuration de la base de données
builder.Services.AddDbContext<BoutiqueEnLigneContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuration de l'authentification
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/GestionDuCompte/Connexion";
        options.LogoutPath = "/GestionDuCompte/Deconnexion";
    });

// Configuration de Stripe
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe")); 
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

// Configuration des services HTTP
builder.Services.AddHttpClient();

// Enregistrement des services
builder.Services.AddScoped<ProductApiService>();
builder.Services.AddScoped<AuthApiService>();
builder.Services.AddScoped<UserApiService>();

// Ajouter la configuration de la session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// En développement, on désactive temporairement la redirection HTTPS
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Ajouter l'utilisation de la session
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Accueil}/{action=Index}/{id?}");

app.Run();