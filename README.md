# Configuration des clés API

## Configuration de Stripe

Pour configurer les clés Stripe, suivez ces étapes :

1. Créez un fichier `appsettings.Development.json` à la racine du projet
2. Ajoutez vos clés Stripe dans ce fichier :

```json
{
  "Stripe": {
    "SecretKey": "votre_clé_secrète",
    "PublishableKey": "votre_clé_publique"
  }
}
```

### Important
- Ne partagez jamais vos clés API dans un dépôt public
- Utilisez des clés de test pour le développement
- Utilisez des clés de production uniquement en production
- Le fichier `appsettings.Development.json` est automatiquement ignoré par Git

## Sécurité

- Les clés API sont stockées dans des fichiers de configuration séparés
- Les fichiers contenant des clés sont exclus du contrôle de version
- Utilisez des variables d'environnement en production 