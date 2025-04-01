using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoutiqueEnLigne.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientVendeurs_Client_ClientId",
                table: "ClientVendeurs");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientVendeurs_Vendeur_VendeurId",
                table: "ClientVendeurs");

            migrationBuilder.DropForeignKey(
                name: "FK_Commandes_Client_ClientId",
                table: "Commandes");

            migrationBuilder.DropForeignKey(
                name: "FK_Commandes_Vendeur_VendeurId",
                table: "Commandes");

            migrationBuilder.DropForeignKey(
                name: "FK_Factures_Client_ClientId",
                table: "Factures");

            migrationBuilder.DropForeignKey(
                name: "FK_Factures_Vendeur_VendeurId",
                table: "Factures");

            migrationBuilder.DropForeignKey(
                name: "FK_Paniers_Client_ClientId",
                table: "Paniers");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Client_ClientAchatId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Client_ClientFavoriId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Client_ClientId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Vendeur_VendeurId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Banks_BankId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_CartesDeCredit_CarteDeCreditId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Vendeur");

            migrationBuilder.DropIndex(
                name: "IX_Users_BankId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CarteDeCreditId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CompanyId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Adresse",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CarteDeCreditId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CodePostal",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Users");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateInscription",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DerniereConnexion",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEntreprise",
                table: "Users",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Users",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "EstActif",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "InscritNewsletter",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomEntreprise",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NotificationsEmail",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumeroClient",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroSIRET",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumeroVendeur",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SiteWeb",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Vendeur_DateInscription",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Vendeur_EstActif",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientVendeurs_Users_ClientId",
                table: "ClientVendeurs",
                column: "ClientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientVendeurs_Users_VendeurId",
                table: "ClientVendeurs",
                column: "VendeurId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Commandes_Users_ClientId",
                table: "Commandes",
                column: "ClientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Commandes_Users_VendeurId",
                table: "Commandes",
                column: "VendeurId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Factures_Users_ClientId",
                table: "Factures",
                column: "ClientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Factures_Users_VendeurId",
                table: "Factures",
                column: "VendeurId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Paniers_Users_ClientId",
                table: "Paniers",
                column: "ClientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_ClientAchatId",
                table: "Products",
                column: "ClientAchatId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_ClientFavoriId",
                table: "Products",
                column: "ClientFavoriId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_ClientId",
                table: "Products",
                column: "ClientId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_VendeurId",
                table: "Products",
                column: "VendeurId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientVendeurs_Users_ClientId",
                table: "ClientVendeurs");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientVendeurs_Users_VendeurId",
                table: "ClientVendeurs");

            migrationBuilder.DropForeignKey(
                name: "FK_Commandes_Users_ClientId",
                table: "Commandes");

            migrationBuilder.DropForeignKey(
                name: "FK_Commandes_Users_VendeurId",
                table: "Commandes");

            migrationBuilder.DropForeignKey(
                name: "FK_Factures_Users_ClientId",
                table: "Factures");

            migrationBuilder.DropForeignKey(
                name: "FK_Factures_Users_VendeurId",
                table: "Factures");

            migrationBuilder.DropForeignKey(
                name: "FK_Paniers_Users_ClientId",
                table: "Paniers");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_ClientAchatId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_ClientFavoriId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_ClientId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_VendeurId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DateInscription",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DerniereConnexion",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DescriptionEntreprise",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EstActif",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "InscritNewsletter",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NomEntreprise",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NotificationsEmail",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NumeroClient",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NumeroSIRET",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NumeroVendeur",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SiteWeb",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Vendeur_DateInscription",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Vendeur_EstActif",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Adresse",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BankId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CarteDeCreditId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CodePostal",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarteDeCreditId = table.Column<int>(type: "int", nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodePostal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateInscription = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DerniereConnexion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstActif = table.Column<bool>(type: "bit", nullable: false),
                    InscritNewsletter = table.Column<bool>(type: "bit", nullable: false),
                    MotDePasse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotificationsEmail = table.Column<bool>(type: "bit", nullable: false),
                    NumeroClient = table.Column<int>(type: "int", nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Client_CartesDeCredit_CarteDeCreditId",
                        column: x => x.CarteDeCreditId,
                        principalTable: "CartesDeCredit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vendeur",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarteDeCreditId = table.Column<int>(type: "int", nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodePostal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateInscription = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DescriptionEntreprise = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstActif = table.Column<bool>(type: "bit", nullable: false),
                    MotDePasse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomEntreprise = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NumeroSIRET = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroVendeur = table.Column<int>(type: "int", nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SiteWeb = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendeur", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vendeur_CartesDeCredit_CarteDeCreditId",
                        column: x => x.CarteDeCreditId,
                        principalTable: "CartesDeCredit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_BankId",
                table: "Users",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CarteDeCreditId",
                table: "Users",
                column: "CarteDeCreditId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyId",
                table: "Users",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_CarteDeCreditId",
                table: "Client",
                column: "CarteDeCreditId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendeur_CarteDeCreditId",
                table: "Vendeur",
                column: "CarteDeCreditId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientVendeurs_Client_ClientId",
                table: "ClientVendeurs",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientVendeurs_Vendeur_VendeurId",
                table: "ClientVendeurs",
                column: "VendeurId",
                principalTable: "Vendeur",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Commandes_Client_ClientId",
                table: "Commandes",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Commandes_Vendeur_VendeurId",
                table: "Commandes",
                column: "VendeurId",
                principalTable: "Vendeur",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Factures_Client_ClientId",
                table: "Factures",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Factures_Vendeur_VendeurId",
                table: "Factures",
                column: "VendeurId",
                principalTable: "Vendeur",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Paniers_Client_ClientId",
                table: "Paniers",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Client_ClientAchatId",
                table: "Products",
                column: "ClientAchatId",
                principalTable: "Client",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Client_ClientFavoriId",
                table: "Products",
                column: "ClientFavoriId",
                principalTable: "Client",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Client_ClientId",
                table: "Products",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Vendeur_VendeurId",
                table: "Products",
                column: "VendeurId",
                principalTable: "Vendeur",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Banks_BankId",
                table: "Users",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "BankId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_CartesDeCredit_CarteDeCreditId",
                table: "Users",
                column: "CarteDeCreditId",
                principalTable: "CartesDeCredit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
