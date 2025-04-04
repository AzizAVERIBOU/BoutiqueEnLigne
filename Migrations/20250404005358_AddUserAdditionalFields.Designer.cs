﻿// <auto-generated />
using System;
using BoutiqueEnLigne.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BoutiqueEnLigne.Migrations
{
    [DbContext(typeof(BoutiqueEnLigneContext))]
    [Migration("20250404005358_AddUserAdditionalFields")]
    partial class AddUserAdditionalFields
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BoutiqueEnLigne.Models.CarteDeCredit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CodeSecurite")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateExpiration")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Numero")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Titulaire")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CartesDeCredit");
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.ClientVendeur", b =>
                {
                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<int>("VendeurId")
                        .HasColumnType("int");

                    b.HasKey("ClientId", "VendeurId");

                    b.HasIndex("VendeurId");

                    b.ToTable("ClientVendeur");
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.Commande", b =>
                {
                    b.Property<int>("CommandeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommandeId"));

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCommande")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("PrixTotal")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Statut")
                        .HasColumnType("int");

                    b.Property<int>("Validation")
                        .HasColumnType("int");

                    b.Property<int>("VendeurId")
                        .HasColumnType("int");

                    b.HasKey("CommandeId");

                    b.HasIndex("ClientId");

                    b.HasIndex("VendeurId");

                    b.ToTable("Commandes");
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.Facture", b =>
                {
                    b.Property<int>("FactureId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FactureId"));

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<int>("CommandeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateFacturation")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModeDePaiement")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("MontantHT")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("NumeroFacture")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PrixTotal")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TVA")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("VendeurId")
                        .HasColumnType("int");

                    b.HasKey("FactureId");

                    b.HasIndex("ClientId");

                    b.HasIndex("CommandeId")
                        .IsUnique();

                    b.HasIndex("VendeurId");

                    b.ToTable("Factures");
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.Panier", b =>
                {
                    b.Property<int>("PanierId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PanierId"));

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreation")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateModification")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("PrixTotal")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantite")
                        .HasColumnType("int");

                    b.HasKey("PanierId");

                    b.HasIndex("ClientId")
                        .IsUnique();

                    b.ToTable("Paniers");
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.Product", b =>
                {
                    b.Property<int>("ProduitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProduitId"));

                    b.Property<string>("Categorie")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "category");

                    b.Property<int?>("ClientAchatId")
                        .HasColumnType("int");

                    b.Property<int?>("ClientFavoriId")
                        .HasColumnType("int");

                    b.Property<int?>("ClientId")
                        .HasColumnType("int");

                    b.Property<int?>("CommandeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateAjout")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateModification")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasAnnotation("Relational:JsonPropertyName", "description");

                    b.Property<int>("Disponibilite")
                        .HasColumnType("int");

                    b.Property<int?>("FactureId")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "thumbnail");

                    b.PrimitiveCollection<string>("Images")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "images");

                    b.Property<string>("Marque")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "brand");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasAnnotation("Relational:JsonPropertyName", "title");

                    b.Property<decimal>("Note")
                        .HasPrecision(3, 2)
                        .HasColumnType("decimal(3,2)")
                        .HasAnnotation("Relational:JsonPropertyName", "rating");

                    b.Property<int?>("PanierId")
                        .HasColumnType("int");

                    b.Property<decimal>("PourcentageReduction")
                        .HasPrecision(5, 2)
                        .HasColumnType("decimal(5,2)")
                        .HasAnnotation("Relational:JsonPropertyName", "discountPercentage");

                    b.Property<decimal>("Prix")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)")
                        .HasAnnotation("Relational:JsonPropertyName", "price");

                    b.Property<int>("Quantite")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "stock");

                    b.Property<int>("VendeurId")
                        .HasColumnType("int");

                    b.HasKey("ProduitId");

                    b.HasIndex("ClientAchatId");

                    b.HasIndex("ClientFavoriId");

                    b.HasIndex("ClientId");

                    b.HasIndex("CommandeId");

                    b.HasIndex("FactureId");

                    b.HasIndex("PanierId");

                    b.HasIndex("VendeurId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.User.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AddressId"));

                    b.Property<string>("Address1")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CoordinatesId")
                        .HasColumnType("int");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("AddressId");

                    b.HasIndex("CoordinatesId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Addresses");

                    b.HasAnnotation("Relational:JsonPropertyName", "address");
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.User.Bank", b =>
                {
                    b.Property<int>("BankId")
                        .HasColumnType("int");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "bankName");

                    b.Property<string>("CardExpire")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "cardExpire");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "cardNumber");

                    b.Property<string>("CardType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "cardType");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "currency");

                    b.Property<string>("Iban")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "iban");

                    b.HasKey("BankId");

                    b.ToTable("Banks");
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.User.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int?>("AddressId")
                        .HasColumnType("int");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "department");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "title");

                    b.HasKey("CompanyId");

                    b.HasIndex("AddressId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.User.Coordinates", b =>
                {
                    b.Property<int>("CoordinatesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CoordinatesId"));

                    b.Property<double>("Latitude")
                        .HasColumnType("float")
                        .HasAnnotation("Relational:JsonPropertyName", "lat");

                    b.Property<double>("Longitude")
                        .HasColumnType("float")
                        .HasAnnotation("Relational:JsonPropertyName", "lng");

                    b.HasKey("CoordinatesId");

                    b.ToTable("Coordinates");
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.User.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateInscription")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateNaissance")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DerniereConnexion")
                        .HasColumnType("datetime2");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("EstActif")
                        .HasColumnType("bit");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("InscritNewsletter")
                        .HasColumnType("bit");

                    b.Property<string>("MotDePasse")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("NotificationsEmail")
                        .HasColumnType("bit");

                    b.Property<string>("Prenom")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("SiteWeb")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telephone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasDiscriminator().HasValue("User");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.Client", b =>
                {
                    b.HasBaseType("BoutiqueEnLigne.Models.User.User");

                    b.Property<int>("NumeroClient")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("Client");
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.Vendeur", b =>
                {
                    b.HasBaseType("BoutiqueEnLigne.Models.User.User");

                    b.Property<string>("DescriptionEntreprise")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("NomEntreprise")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NumeroSIRET")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumeroVendeur")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("Vendeur");
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.ClientVendeur", b =>
                {
                    b.HasOne("BoutiqueEnLigne.Models.Client", "Client")
                        .WithMany("ClientVendeurs")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BoutiqueEnLigne.Models.Vendeur", "Vendeur")
                        .WithMany("ClientVendeurs")
                        .HasForeignKey("VendeurId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Vendeur");
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.Commande", b =>
                {
                    b.HasOne("BoutiqueEnLigne.Models.Client", "Client")
                        .WithMany("Commandes")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BoutiqueEnLigne.Models.Vendeur", "Vendeur")
                        .WithMany("Commandes")
                        .HasForeignKey("VendeurId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Vendeur");
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.Facture", b =>
                {
                    b.HasOne("BoutiqueEnLigne.Models.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BoutiqueEnLigne.Models.Commande", "Commande")
                        .WithOne("Facture")
                        .HasForeignKey("BoutiqueEnLigne.Models.Facture", "CommandeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BoutiqueEnLigne.Models.Vendeur", "Vendeur")
                        .WithMany()
                        .HasForeignKey("VendeurId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Commande");

                    b.Navigation("Vendeur");
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.Panier", b =>
                {
                    b.HasOne("BoutiqueEnLigne.Models.Client", "Client")
                        .WithOne("Panier")
                        .HasForeignKey("BoutiqueEnLigne.Models.Panier", "ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.Product", b =>
                {
                    b.HasOne("BoutiqueEnLigne.Models.Client", "ClientAchat")
                        .WithMany("ProduitsAchat")
                        .HasForeignKey("ClientAchatId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BoutiqueEnLigne.Models.Client", "ClientFavori")
                        .WithMany("ProduitsFavoris")
                        .HasForeignKey("ClientFavoriId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BoutiqueEnLigne.Models.Client", "Client")
                        .WithMany("Produits")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BoutiqueEnLigne.Models.Commande", null)
                        .WithMany("Produits")
                        .HasForeignKey("CommandeId");

                    b.HasOne("BoutiqueEnLigne.Models.Facture", null)
                        .WithMany("Produits")
                        .HasForeignKey("FactureId");

                    b.HasOne("BoutiqueEnLigne.Models.Panier", null)
                        .WithMany("Produits")
                        .HasForeignKey("PanierId");

                    b.HasOne("BoutiqueEnLigne.Models.Vendeur", "Vendeur")
                        .WithMany("ProduitsVendus")
                        .HasForeignKey("VendeurId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("ClientAchat");

                    b.Navigation("ClientFavori");

                    b.Navigation("Vendeur");
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.User.Address", b =>
                {
                    b.HasOne("BoutiqueEnLigne.Models.User.Coordinates", "Coordinates")
                        .WithMany()
                        .HasForeignKey("CoordinatesId");

                    b.HasOne("BoutiqueEnLigne.Models.User.User", "User")
                        .WithOne("Address")
                        .HasForeignKey("BoutiqueEnLigne.Models.User.Address", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coordinates");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.User.Bank", b =>
                {
                    b.HasOne("BoutiqueEnLigne.Models.User.User", null)
                        .WithOne("Bank")
                        .HasForeignKey("BoutiqueEnLigne.Models.User.Bank", "BankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.User.Company", b =>
                {
                    b.HasOne("BoutiqueEnLigne.Models.User.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.HasOne("BoutiqueEnLigne.Models.User.User", null)
                        .WithOne("Company")
                        .HasForeignKey("BoutiqueEnLigne.Models.User.Company", "CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.Commande", b =>
                {
                    b.Navigation("Facture")
                        .IsRequired();

                    b.Navigation("Produits");
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.Facture", b =>
                {
                    b.Navigation("Produits");
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.Panier", b =>
                {
                    b.Navigation("Produits");
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.User.User", b =>
                {
                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("Bank")
                        .IsRequired();

                    b.Navigation("Company")
                        .IsRequired();
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.Client", b =>
                {
                    b.Navigation("ClientVendeurs");

                    b.Navigation("Commandes");

                    b.Navigation("Panier")
                        .IsRequired();

                    b.Navigation("Produits");

                    b.Navigation("ProduitsAchat");

                    b.Navigation("ProduitsFavoris");
                });

            modelBuilder.Entity("BoutiqueEnLigne.Models.Vendeur", b =>
                {
                    b.Navigation("ClientVendeurs");

                    b.Navigation("Commandes");

                    b.Navigation("ProduitsVendus");
                });
#pragma warning restore 612, 618
        }
    }
}
