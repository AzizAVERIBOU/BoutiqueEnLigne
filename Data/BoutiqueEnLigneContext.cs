using BoutiqueEnLigne.Models;
using BoutiqueEnLigne.Models.User;
using Microsoft.EntityFrameworkCore;

namespace BoutiqueEnLigne.Data
{
    public class BoutiqueEnLigneContext : DbContext
    {
        public BoutiqueEnLigneContext(DbContextOptions<BoutiqueEnLigneContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Coordinates> Coordinates { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<Facture> Factures { get; set; }
        public DbSet<Panier> Paniers { get; set; }
        public DbSet<CarteDeCredit> CartesDeCredit { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration de la relation User-Address
            modelBuilder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration de l'email unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configuration de la relation Client-Vendeur (many-to-many)
            modelBuilder.Entity<ClientVendeur>()
                .HasKey(cv => new { cv.ClientId, cv.VendeurId });

            modelBuilder.Entity<ClientVendeur>()
                .HasOne(cv => cv.Client)
                .WithMany(c => c.ClientVendeurs)
                .HasForeignKey(cv => cv.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClientVendeur>()
                .HasOne(cv => cv.Vendeur)
                .WithMany(v => v.ClientVendeurs)
                .HasForeignKey(cv => cv.VendeurId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration des autres relations
            modelBuilder.Entity<Bank>()
                .HasOne<User>()
                .WithOne(u => u.Bank)
                .HasForeignKey<Bank>(b => b.BankId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Company>()
                .HasOne<User>()
                .WithOne(u => u.Company)
                .HasForeignKey<Company>(c => c.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Panier>()
                .HasOne(p => p.Client)
                .WithOne(c => c.Panier)
                .HasForeignKey<Panier>(p => p.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration de la relation Commande-Client et Commande-Vendeur
            modelBuilder.Entity<Commande>()
                .HasOne(c => c.Client)
                .WithMany(u => u.Commandes)
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Commande>()
                .HasOne(c => c.Vendeur)
                .WithMany(v => v.Commandes)
                .HasForeignKey(c => c.VendeurId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration de la relation Facture-Commande
            modelBuilder.Entity<Facture>()
                .HasOne(f => f.Commande)
                .WithOne(c => c.Facture)
                .HasForeignKey<Facture>(f => f.CommandeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration de la relation Facture-Client et Facture-Vendeur
            modelBuilder.Entity<Facture>()
                .HasOne(f => f.Client)
                .WithMany()
                .HasForeignKey(f => f.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Facture>()
                .HasOne(f => f.Vendeur)
                .WithMany()
                .HasForeignKey(f => f.VendeurId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration des propriétés décimales
            modelBuilder.Entity<Commande>()
                .Property(c => c.PrixTotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Facture>()
                .Property(f => f.PrixTotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Facture>()
                .Property(f => f.MontantHT)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Facture>()
                .Property(f => f.TVA)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Panier>()
                .Property(p => p.PrixTotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.Prix)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.PourcentageReduction)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.Note)
                .HasPrecision(3, 2);

            // Configuration des relations Product
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Vendeur)
                .WithMany(v => v.ProduitsVendus)
                .HasForeignKey(p => p.VendeurId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Client)
                .WithMany(c => c.Produits)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.ClientFavori)
                .WithMany(c => c.ProduitsFavoris)
                .HasForeignKey(p => p.ClientFavoriId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.ClientAchat)
                .WithMany(c => c.ProduitsAchat)
                .HasForeignKey(p => p.ClientAchatId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 