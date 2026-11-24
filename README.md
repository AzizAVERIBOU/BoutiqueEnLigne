# BoutiqueEnLigne

Travail pratique réalisé dans un cadre scolaire par Aziz AVERIBOU et Mor DIONGUE.

Application web de boutique en ASP.NET Core MVC. Un visiteur parcourt un catalogue, constitue un panier et peut payer avec Stripe. Un vendeur inscrit enregistre ses propres articles dans SQL Server.

Dépôt : [github.com/AzizAVERIBOU/BoutiqueEnLigne](https://github.com/AzizAVERIBOU/BoutiqueEnLigne)

## Parcours

### Visiteur

- L'accueil liste les produits, avec pagination.
- Une fiche détaille un article.
- Les catégories et la recherche filtrent le catalogue.
- Le panier et les favoris sont gardés dans la session.

Ces écrans lisent le catalogue public de [DummyJSON](https://dummyjson.com).

### Compte

- Inscription avec le rôle Client ou Vendeur.
- Connexion par cookie.
- Profil, adresses et page de sécurité.

Les mots de passe enregistrés par l'application sont hachés avec BCrypt.

### Vendeur

Après connexion avec le rôle Vendeur :

- `/Vendeur` liste les articles de ce vendeur ;
- `/Vendeur/AjouterProduit` enregistre un article : nom, description, prix, stock, catégorie et URL d'image.

Ces articles sont écrits dans la table `ProduitsVendeurs`. L'identifiant du vendeur est celui du compte connecté, dans `Users`. La vitrine ne lit pas encore cette table : les deux catalogues sont séparés.

### Paiement

Le panier ouvre une session Stripe Checkout. Les clés ne sont pas dans le dépôt. Sans clé secrète, la session n'est pas créée.

## Pile

| Rôle | Choix |
| --- | --- |
| Application | ASP.NET Core MVC, .NET 8 |
| Données | Entity Framework Core 9, SQL Server LocalDB |
| Mots de passe | BCrypt.Net-Next |
| Paiement | Stripe.net |
| Interface | Razor, Bootstrap 5.3, Bootstrap Icons, jQuery |
| Catalogue visiteur | API DummyJSON |
| Tests | xUnit, deux tests de fumée |
| Intégration continue | GitHub Actions sur `main` et `develop` |

## Démarrage

Prérequis : SDK .NET 8 et SQL Server LocalDB.

```bash
git clone https://github.com/AzizAVERIBOU/BoutiqueEnLigne.git
cd BoutiqueEnLigne
dotnet restore BoutiqueEnLigne.sln --locked-mode
dotnet ef database update --project BoutiqueEnLigne.csproj
dotnet run --launch-profile http
```

L'application écoute sur http://localhost:5118.

Pour essayer l'espace vendeur : s'inscrire avec le rôle Vendeur, se connecter, puis ouvrir **Ajouter un produit**.

La commande `dotnet ef` demande l'outil Entity Framework installé sur la machine (`dotnet tool install --global dotnet-ef`).

### Clés Stripe

`appsettings.json` laisse les clés vides. En développement :

```bash
dotnet user-secrets set "Stripe:SecretKey" "sk_test_..."
dotnet user-secrets set "Stripe:PublishableKey" "pk_test_..."
```

La chaîne de connexion LocalDB reste dans `appsettings.json`.

### Tests

```bash
dotnet test BoutiqueEnLigne.sln --configuration Release
```

Le pipeline restaure les paquets en mode verrouillé, compile en Release, lance les tests et publie l'application.

## Code

```
BoutiqueEnLigne/
├── Controllers/          Accueil, compte, paiement, recherche, vendeur
├── Models/               utilisateurs, ProduitVendeur, commandes
├── Views/                pages Razor
├── Services/             appels à DummyJSON
├── Data/                 contexte Entity Framework
├── Migrations/           schéma SQL Server
├── tests/                tests de fumée
└── .github/workflows/    pipeline
```

Les branches de travail partent de `develop`. `main` reste la ligne stable.

## Licence

MIT. Voir [LICENSE](LICENSE).

## Portfolio

[profil-aziz.vercel.app](https://profil-aziz.vercel.app/)
