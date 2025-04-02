using Microsoft.EntityFrameworkCore;
using BoutiqueEnLigne.Models;
using BoutiqueEnLigne.Models.User;
using BoutiqueEnLigne.Data;
using BoutiqueEnLigne.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

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

// Configuration des services HTTP
builder.Services.AddHttpClient();

// Enregistrement des services
builder.Services.AddScoped<ProductApiService>();
builder.Services.AddScoped<AuthApiService>();
builder.Services.AddScoped<UserApiService>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Accueil}/{action=Index}/{id?}");

app.Run();