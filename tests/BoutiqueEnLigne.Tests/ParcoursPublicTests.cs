using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BoutiqueEnLigne.Tests;

public class ParcoursPublicTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ParcoursPublicTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseSetting(
                "ConnectionStrings:DefaultConnection",
                "Server=localhost;Database=BoutiqueEnLigneCi;Trusted_Connection=True;TrustServerCertificate=True");
            builder.UseSetting("Stripe:SecretKey", "");
            builder.UseSetting("Stripe:PublishableKey", "");
        });
    }

    [Fact]
    public async Task APropos_RepondOk()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/Autres/APropos");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("À propos de Notre Boutique", html);
    }

    [Fact]
    public async Task InfosPersonnelles_ExigeLaConnexion()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var response = await client.GetAsync("/GestionDuCompte/Infospersonnelles");

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/GestionDuCompte/Connexion", response.Headers.Location?.OriginalString);
    }
}
