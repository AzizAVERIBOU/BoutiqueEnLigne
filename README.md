# BoutiqueEnLigne

Une application de commerce électronique moderne développée avec ASP.NET Core MVC, offrant une expérience d'achat complète avec gestion des utilisateurs, des produits, des commandes et des paiements.

## Table des matières

- [Fonctionnalités](#-fonctionnalités)
- [Technologies utilisées](#-technologies-utilisées)
- [Prérequis](#-prérequis)
- [Installation](#-installation)
- [Configuration](#-configuration)
- [Utilisation](#-utilisation)
- [Structure du projet](#-structure-du-projet)
- [API et services externes](#-api-et-services-externes)
- [Base de données](#-base-de-données)
- [Déploiement](#-déploiement)
- [Contribution](#-contribution)
- [Licence](#-licence)

## Fonctionnalités

### Commerce électronique
- **Catalogue de produits** avec pagination et filtrage par catégories
- **Recherche avancée** par nom, description, marque et catégorie
- **Panier d'achat** persistant avec gestion des quantités
- **Système de favoris** pour sauvegarder les produits préférés
- **Paiement sécurisé** via Stripe Checkout
- **Gestion des commandes** avec suivi des statuts

### Gestion des utilisateurs
- **Inscription et connexion** avec validation des données
- **Rôles multiples** : Client, Vendeur, Administrateur
- **Profil utilisateur** avec informations personnelles
- **Gestion des adresses** et informations de livraison
- **Historique des commandes** et factures

### Espace vendeur
- **Tableau de bord** pour les vendeurs
- **Gestion des produits** (ajout, modification, suppression)
- **Suivi des ventes** et statistiques
- **Gestion des commandes** reçues

### Sécurité
- **Authentification par cookies** avec sessions persistantes
- **Hachage des mots de passe** avec BCrypt
- **Validation des données** côté serveur et client
- **Protection CSRF** avec tokens anti-forgery
- **Gestion des rôles** et autorisations

## Technologies utilisées

### Arrière-plan
- **ASP.NET Core MVC 8.0** - Cadre d'application web principal
- **Entity Framework Core 9.0.3** - ORM pour la base de données
- **SQL Server** - Base de données relationnelle
- **BCrypt.Net-Next 4.0.3** - Hachage sécurisé des mots de passe
- **Stripe.net 48.0.0** - Intégration des paiements

### Interface utilisateur
- **Bootstrap 5.3.2** - Cadre CSS adaptatif
- **jQuery 3.7.1** - Bibliothèque JavaScript
- **Bootstrap Icons** - Icônes vectorielles
- **AJAX** - Interactions asynchrones

### API externes
- **DummyJSON** - Données de produits et utilisateurs de test

## Prérequis

- **.NET 8.0 SDK** ou version ultérieure
- **SQL Server** (LocalDB, Express ou version complète)
- **Visual Studio 2022** ou **Visual Studio Code**
- **Git** pour le contrôle de versions

## Installation

### 1. Cloner le dépôt
```bash
git clone https://github.com/votre-username/BoutiqueEnLigne.git
cd BoutiqueEnLigne
```

### 2. Restaurer les paquets NuGet
```bash
dotnet restore
```

### 3. Configurer la base de données
```bash
# Créer la base de données
dotnet ef database update
```

### 4. Démarrer l'application
```bash
dotnet run
```

L'application sera accessible à l'adresse : `https://localhost:7000`

## Configuration

### 1. Configuration de la base de données

Modifiez le fichier `appsettings.json` :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BoutiqueEnLigne;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"
  }
}
```

### 2. Configuration de Stripe

Créez un fichier `appsettings.Development.json` :

```json
{
  "Stripe": {
    "SecretKey": "sk_test_votre_cle_secrete_stripe",
    "PublishableKey": "pk_test_votre_cle_publique_stripe"
  }
}
```

### 3. Variables d'environnement (Production)

```bash
export Stripe__SecretKey="sk_live_votre_cle_secrete_production"
export Stripe__PublishableKey="pk_live_votre_cle_publique_production"
```

## Utilisation

### 👤 Comptes de test

#### Client
- **Email** : `john@example.com`
- **Mot de passe** : `password123`

#### Vendeur
- **Email** : `jane@example.com`
- **Mot de passe** : `password123`

### Parcours d'achat

1. **Navigation** : Parcourez les produits par catégorie ou utilisez la recherche
2. **Sélection** : Ajoutez des produits au panier ou aux favoris
3. **Connexion** : Connectez-vous ou créez un compte
4. **Commande** : Validez votre panier et procédez au paiement
5. **Paiement** : Utilisez Stripe Checkout pour finaliser l'achat

### 🏪 Espace vendeur

1. **Connexion** : Connectez-vous avec un compte vendeur
2. **Produits** : Gérez votre catalogue de produits
3. **Ventes** : Consultez vos statistiques de ventes
4. **Commandes** : Traitez les commandes reçues

## Structure du projet

```
BoutiqueEnLigne/
├── Controllers/              # Contrôleurs MVC
│   ├── AccueilController.cs     # Page d'accueil et produits
│   ├── AccountController.cs     # Authentification
│   ├── GestionDuCompteController.cs # Gestion du compte utilisateur
│   ├── PaiementController.cs    # Paiements et panier
│   ├── ProduitController.cs     # Recherche de produits
│   └── VendeurController.cs     # Espace vendeur
├── Models/                   # Modèles de données
│   ├── User/                    # Modèles utilisateur
│   ├── ViewModels/              # Modèles de vue
│   ├── Product.cs               # Modèle produit
│   ├── Commande.cs              # Modèle commande
│   └── Enums.cs                 # Énumérations
├── Views/                    # Vues Razor
│   ├── Accueil/                 # Vues de la page d'accueil
│   ├── GestionDuCompte/         # Vues de gestion du compte
│   ├── Paiement/                # Vues de paiement
│   ├── Shared/                  # Vues partagées
│   └── Vendeur/                 # Vues de l'espace vendeur
├── Services/                 # Services métier
│   ├── ProductApiService.cs     # Service produits API
│   ├── AuthApiService.cs        # Service authentification
│   └── UserApiService.cs        # Service utilisateurs
├── Data/                     # Accès aux données
│   └── BoutiqueEnLigneContext.cs # Contexte Entity Framework
├── Migrations/               # Migrations de base de données
├── wwwroot/                  # Fichiers statiques
│   ├── css/                     # Feuilles de style
│   ├── js/                      # Scripts JavaScript
│   └── images/                  # Images
└── Properties/               # Configuration du projet
```

## 🔌 API et services externes

### DummyJSON
L'application utilise l'API DummyJSON pour :
- **Produits** : Catalogue de produits de test
- **Utilisateurs** : Données utilisateur pour l'authentification
- **Base URL** : `https://dummyjson.com`

### Stripe
Intégration complète de Stripe pour :
- **Paiements** : Traitement sécurisé des transactions
- **Checkout** : Interface de paiement intégrée
- **Webhooks** : Notifications de paiement (à implémenter)

## Base de données

### Modèles principaux

#### Users
- **User** : Classe de base avec informations communes
- **Client** : Utilisateur avec fonctionnalités d'achat
- **Vendeur** : Utilisateur avec fonctionnalités de vente

#### Produits et commandes
- **Product** : Produits avec images, prix, stock
- **Commande** : Commandes avec statuts et validation
- **Facture** : Factures avec TVA et montants
- **Panier** : Panier utilisateur (session)

#### Relations
- **ClientVendeur** : Relation many-to-many
- **Address, Bank, Company** : Informations utilisateur

### Migrations
```bash
# Créer une nouvelle migration
dotnet ef migrations add NomDeLaMigration

# Appliquer les migrations
dotnet ef database update

# Supprimer la dernière migration
dotnet ef migrations remove
```

## Déploiement

### Déploiement local
```bash
# Build en mode Release
dotnet build --configuration Release

# Publier l'application
dotnet publish --configuration Release --output ./publish

# Lancer l'application publiée
dotnet ./publish/BoutiqueEnLigne.dll
```

### Déploiement Azure
1. **Azure App Service** : Déployez l'application
2. **Azure SQL Database** : Configurez la base de données
3. **Variables d'environnement** : Configurez Stripe et la connexion DB
4. **SSL** : Activez HTTPS

### Déploiement Docker
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BoutiqueEnLigne.csproj", "."]
RUN dotnet restore
COPY . .
RUN dotnet build --configuration Release --no-restore

FROM build AS publish
RUN dotnet publish --configuration Release --no-build --output /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BoutiqueEnLigne.dll"]
```

## Contribution

### Comment contribuer

1. **Fork** le projet
2. **Créer** une branche pour votre fonctionnalité (`git basculer -b feature/FonctionnaliteGeniale`)
3. **Valider** vos changements (`git commit -m 'Ajouter une fonctionnalité géniale'`)
4. **Pousser** vers la branche (`git push origin feature/FonctionnaliteGeniale`)
5. **Ouvrir** une Demande de tirage

### Standards de code

- **C#** : Suivez les conventions Microsoft
- **CSS** : Utilisez Bootstrap et les classes personnalisées
- **JavaScript** : Code ES6+ avec jQuery
- **Documentation** : Commentez le code complexe

### Problèmes et bugs

- **Rapports de bugs** : Utilisez le modèle d'issue
- **Demandes de fonctionnalités** : Décrivez clairement la fonctionnalité
- **Questions** : Utilisez les discussions GitHub

## 📄 Licence

Ce projet est sous licence MIT. Voir le fichier [LICENSE](LICENSE) pour plus de détails.

## Support

- **Email** : azizaveribou6123@gmail.com ou mordiongue100@gmail.com
- **Documentation** : [Wiki du projet](https://github.com/votre-username/BoutiqueEnLigne/wiki)
- **Problèmes** : [GitHub Issues](https://github.com/votre-username/BoutiqueEnLigne/issues)

## Remerciements

- **Bootstrap** pour le cadre CSS
- **Stripe** pour l'intégration des paiements
- **DummyJSON** pour les données de test
- **Microsoft** pour ASP.NET Core

---

**Développé avec passion en ASP.NET Core**