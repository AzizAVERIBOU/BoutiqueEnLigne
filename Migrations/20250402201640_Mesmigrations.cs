using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoutiqueEnLigne.Migrations
{
    /// <inheritdoc />
    public partial class Mesmigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CartesDeCredit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateExpiration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodeSecurite = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Titulaire = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartesDeCredit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Coordinates",
                columns: table => new
                {
                    CoordinatesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinates", x => x.CoordinatesId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MotDePasse = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateInscription = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DerniereConnexion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    EstActif = table.Column<bool>(type: "bit", nullable: false),
                    NotificationsEmail = table.Column<bool>(type: "bit", nullable: false),
                    InscritNewsletter = table.Column<bool>(type: "bit", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    NumeroClient = table.Column<int>(type: "int", nullable: true),
                    NumeroVendeur = table.Column<int>(type: "int", nullable: true),
                    NomEntreprise = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NumeroSIRET = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEntreprise = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SiteWeb = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoordinatesId = table.Column<int>(type: "int", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_Addresses_Coordinates_CoordinatesId",
                        column: x => x.CoordinatesId,
                        principalTable: "Coordinates",
                        principalColumn: "CoordinatesId");
                    table.ForeignKey(
                        name: "FK_Addresses_Users_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    BankId = table.Column<int>(type: "int", nullable: false),
                    CardExpire = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Iban = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.BankId);
                    table.ForeignKey(
                        name: "FK_Banks_Users_BankId",
                        column: x => x.BankId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClientVendeurs",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    VendeurId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientVendeurs", x => new { x.ClientId, x.VendeurId });
                    table.ForeignKey(
                        name: "FK_ClientVendeurs_Users_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClientVendeurs_Users_VendeurId",
                        column: x => x.VendeurId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Commandes",
                columns: table => new
                {
                    CommandeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCommande = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrixTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Statut = table.Column<int>(type: "int", nullable: false),
                    Validation = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    VendeurId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commandes", x => x.CommandeId);
                    table.ForeignKey(
                        name: "FK_Commandes_Users_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Commandes_Users_VendeurId",
                        column: x => x.VendeurId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Paniers",
                columns: table => new
                {
                    PanierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantite = table.Column<int>(type: "int", nullable: false),
                    PrixTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModification = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paniers", x => x.PanierId);
                    table.ForeignKey(
                        name: "FK_Paniers_Users_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_Companies_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId");
                    table.ForeignKey(
                        name: "FK_Companies_Users_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Factures",
                columns: table => new
                {
                    FactureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrixTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DateFacturation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModeDePaiement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommandeId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    VendeurId = table.Column<int>(type: "int", nullable: false),
                    NumeroFacture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TVA = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MontantHT = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factures", x => x.FactureId);
                    table.ForeignKey(
                        name: "FK_Factures_Commandes_CommandeId",
                        column: x => x.CommandeId,
                        principalTable: "Commandes",
                        principalColumn: "CommandeId");
                    table.ForeignKey(
                        name: "FK_Factures_Users_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Factures_Users_VendeurId",
                        column: x => x.VendeurId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProduitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Prix = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Categorie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantite = table.Column<int>(type: "int", nullable: false),
                    Disponibilite = table.Column<int>(type: "int", nullable: false),
                    VendeurId = table.Column<int>(type: "int", nullable: false),
                    DateAjout = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModification = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PourcentageReduction = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Note = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false),
                    Marque = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    ClientFavoriId = table.Column<int>(type: "int", nullable: true),
                    ClientAchatId = table.Column<int>(type: "int", nullable: true),
                    CommandeId = table.Column<int>(type: "int", nullable: true),
                    FactureId = table.Column<int>(type: "int", nullable: true),
                    PanierId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProduitId);
                    table.ForeignKey(
                        name: "FK_Products_Commandes_CommandeId",
                        column: x => x.CommandeId,
                        principalTable: "Commandes",
                        principalColumn: "CommandeId");
                    table.ForeignKey(
                        name: "FK_Products_Factures_FactureId",
                        column: x => x.FactureId,
                        principalTable: "Factures",
                        principalColumn: "FactureId");
                    table.ForeignKey(
                        name: "FK_Products_Paniers_PanierId",
                        column: x => x.PanierId,
                        principalTable: "Paniers",
                        principalColumn: "PanierId");
                    table.ForeignKey(
                        name: "FK_Products_Users_ClientAchatId",
                        column: x => x.ClientAchatId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_Users_ClientFavoriId",
                        column: x => x.ClientFavoriId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_Users_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_Users_VendeurId",
                        column: x => x.VendeurId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CoordinatesId",
                table: "Addresses",
                column: "CoordinatesId",
                unique: true,
                filter: "[CoordinatesId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClientVendeurs_VendeurId",
                table: "ClientVendeurs",
                column: "VendeurId");

            migrationBuilder.CreateIndex(
                name: "IX_Commandes_ClientId",
                table: "Commandes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Commandes_VendeurId",
                table: "Commandes",
                column: "VendeurId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_AddressId",
                table: "Companies",
                column: "AddressId",
                unique: true,
                filter: "[AddressId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Factures_ClientId",
                table: "Factures",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Factures_CommandeId",
                table: "Factures",
                column: "CommandeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Factures_VendeurId",
                table: "Factures",
                column: "VendeurId");

            migrationBuilder.CreateIndex(
                name: "IX_Paniers_ClientId",
                table: "Paniers",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ClientAchatId",
                table: "Products",
                column: "ClientAchatId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ClientFavoriId",
                table: "Products",
                column: "ClientFavoriId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ClientId",
                table: "Products",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CommandeId",
                table: "Products",
                column: "CommandeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_FactureId",
                table: "Products",
                column: "FactureId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_PanierId",
                table: "Products",
                column: "PanierId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_VendeurId",
                table: "Products",
                column: "VendeurId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "CartesDeCredit");

            migrationBuilder.DropTable(
                name: "ClientVendeurs");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Factures");

            migrationBuilder.DropTable(
                name: "Paniers");

            migrationBuilder.DropTable(
                name: "Coordinates");

            migrationBuilder.DropTable(
                name: "Commandes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
