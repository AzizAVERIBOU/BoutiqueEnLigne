//using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);

var app = builder.Build();

app.UseStaticFiles();
 
app.UseMvc(routes => routes.MapRoute(
    name: "default",
    template: "{controller=Accueil}/{action=Accueil}/{id?}"));

app.Run();