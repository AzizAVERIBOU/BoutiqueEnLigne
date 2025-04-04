function updateCartBadge(count) {
    const badge = document.querySelector('.cart-badge');
    if (count > 0) {
        if (!badge) {
            const cartLink = document.querySelector('.nav-link[href*="Panier"]');
            const newBadge = document.createElement('span');
            newBadge.className = 'badge bg-danger rounded-pill cart-badge';
            newBadge.textContent = count;
            cartLink.appendChild(newBadge);
        } else {
            badge.textContent = count;
        }
    } else if (badge) {
        badge.remove();
    }
}

// Fonction pour ajouter au panier
async function ajouterAuPanier(productId) {
    try {
        const response = await fetch('/Panier/AjouterAuPanier', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ productId: productId })
        });

        if (response.ok) {
            const data = await response.json();
            updateCartBadge(data.nombreArticles);
            
            // Afficher un toast de confirmation
            const toast = new bootstrap.Toast(document.getElementById('toastAjoutPanier'));
            toast.show();
        }
    } catch (error) {
        console.error('Erreur lors de l\'ajout au panier:', error);
    }
}

// Fonction pour supprimer du panier
async function supprimerDuPanier(productId) {
    try {
        const response = await fetch('/Panier/SupprimerDuPanier', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ productId: productId })
        });

        if (response.ok) {
            const data = await response.json();
            updateCartBadge(data.nombreArticles);
            
            // Recharger la page du panier si on y est
            if (window.location.pathname.includes('Panier')) {
                window.location.reload();
            }
        }
    } catch (error) {
        console.error('Erreur lors de la suppression du panier:', error);
    }
} 