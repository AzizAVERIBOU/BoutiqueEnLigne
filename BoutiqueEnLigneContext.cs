using Microsoft.EntityFrameworkCore;
using BoutiqueEnLigne.Models;
using BoutiqueEnLigne.Models.User;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoutiqueEnLigne.Data
{
    public class BoutiqueEnLigneContext : DbContext 
    {
        public BoutiqueEnLigneContext(DbContextOptions<BoutiqueEnLigneContext> options) : base(options)
        {
        }

        public void NettoyerBaseDeDonnees()
        {
            // Suppression des données dans l'ordre pour respecter les contraintes de clés étrangères
            Factures.RemoveRange(Factures);
            Commandes.RemoveRange(Commandes);
            Paniers.RemoveRange(Paniers);
            Products.RemoveRange(Products);
            ClientVendeurs.RemoveRange(ClientVendeurs);
            CartesDeCredit.RemoveRange(CartesDeCredit);
            Addresses.RemoveRange(Addresses);
            Coordinates.RemoveRange(Coordinates);
            Banks.RemoveRange(Banks);
            Companies.RemoveRange(Companies);
            Users.RemoveRange(Users);

            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BoutiqueEnLigne;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration des propriétés decimal pour Commande
            modelBuilder.Entity<Commande>()
                .Property(c => c.PrixTotal)
                .HasPrecision(18, 2);

            // Configuration des propriétés decimal pour Facture
            modelBuilder.Entity<Facture>()
                .Property(f => f.PrixTotal)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Facture>()
                .Property(f => f.MontantHT)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Facture>()
                .Property(f => f.TVA)
                .HasPrecision(18, 2);

            // Configuration des propriétés decimal pour Panier
            modelBuilder.Entity<Panier>()
                .Property(p => p.PrixTotal)
                .HasPrecision(18, 2);

            // Configuration des propriétés decimal pour Product
            modelBuilder.Entity<Product>()
                .Property(p => p.Prix)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Product>()
                .Property(p => p.Note)
                .HasPrecision(3, 2);
            modelBuilder.Entity<Product>()
                .Property(p => p.PourcentageReduction)
                .HasPrecision(5, 2);

            // Configuration des relations pour éviter les cycles de suppression
            modelBuilder.Entity<ClientVendeur>()
                .HasKey(cv => new { cv.ClientId, cv.VendeurId });

            modelBuilder.Entity<ClientVendeur>()
                .HasOne(cv => cv.Client)
                .WithMany(c => c.ClientVendeurs)
                .HasForeignKey(cv => cv.ClientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ClientVendeur>()
                .HasOne(cv => cv.Vendeur)
                .WithMany(v => v.ClientVendeurs)
                .HasForeignKey(cv => cv.VendeurId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configuration des autres relations pour éviter les cycles
            modelBuilder.Entity<Client>()
                .HasOne(c => c.Panier)
                .WithOne(p => p.Client)
                .HasForeignKey<Panier>(p => p.ClientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Commande>()
                .HasOne(c => c.Client)
                .WithMany(cl => cl.Commandes)
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Commande>()
                .HasOne(c => c.Vendeur)
                .WithMany(v => v.Commandes)
                .HasForeignKey(c => c.VendeurId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Facture>()
                .HasOne(f => f.Commande)
                .WithOne(c => c.Facture)
                .HasForeignKey<Facture>(f => f.CommandeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Facture>()
                .HasOne(f => f.Client)
                .WithMany()
                .HasForeignKey(f => f.ClientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Facture>()
                .HasOne(f => f.Vendeur)
                .WithMany()
                .HasForeignKey(f => f.VendeurId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configuration des relations pour User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne()
                .HasForeignKey<Address>(a => a.AddressId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Address>()
                .HasOne(a => a.Coordinates)
                .WithOne()
                .HasForeignKey<Address>(a => a.CoordinatesId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Bank)
                .WithOne()
                .HasForeignKey<Bank>(b => b.BankId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Company)
                .WithOne()
                .HasForeignKey<Company>(c => c.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Company>()
                .HasOne(c => c.Address)
                .WithOne()
                .HasForeignKey<Company>(c => c.AddressId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configuration de la propriété Role pour User
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<int>()
                .IsRequired();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Panier> Paniers { get; set; }
        public DbSet<Facture> Factures { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<CarteDeCredit> CartesDeCredit { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Coordinates> Coordinates { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ClientVendeur> ClientVendeurs { get; set; }
    }
}