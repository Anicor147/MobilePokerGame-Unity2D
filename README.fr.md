# Poker mobile

*[English version](README.md)*

Jeu de poker fermé (*five-card draw*) en duel contre l'ordinateur, conçu pour mobile en mode paysage.
Projet académique solo réalisé avec **Unity 2022.3** et **C#** (mars – avril 2024).

## Principe

Une partie se joue en **10 manches**. À chaque manche, le joueur choisit les cartes à conserver, pige pour remplacer les autres, puis sa main est comparée à celle de l'adversaire. La main gagnante rapporte des points selon sa combinaison. Le joueur qui a le plus de points après la 10e manche gagne la partie et de l'or.

L'or sert à acheter des avantages dans la boutique, et le menu principal permet de modifier son paquet de cartes avant la partie.

## Fonctionnalités

- **Évaluation des mains** : détection des 10 combinaisons, de la carte haute à la quinte flush royale, et comparaison avec la main adverse pour désigner le gagnant de la manche.
- **Conservation des cartes** : le joueur touche une carte pour la garder avant de piger. Le nombre de cartes conservables est limité et peut être augmenté en boutique.
- **Boutique en partie** : carte conservable supplémentaire, score doublé pour la manche, or doublé, remise à zéro du score adverse.
- **Construction du paquet** : depuis le menu principal, le joueur retire des cartes de son paquet et ajoute des jokers. Le joker noir rapporte de l'or, le joker rouge retire 5 points à l'adversaire.
- **Persistance entre les scènes** : l'or et le paquet modifié sont conservés dans des ScriptableObjects.
- **Animations** des cartes du joueur et de l'adversaire, écrans de victoire et de défaite, réglage du volume.

## Architecture

| Script | Rôle |
| --- | --- |
| `GameManager` | Boucle de jeu, pioche, évaluation des combinaisons, score et fin de partie |
| `Carte` / `CarteData` | Affichage d'une carte, sélection tactile (`IPointerDownHandler`) |
| `ShopManager` | Boutique en partie et gestion de l'or |
| `MenuShop` | Construction du paquet depuis le menu principal |
| `CardAnimation` | Animations de distribution |
| `SOScripts/*` | ScriptableObjects : sprites, paquet sauvegardé, or, audio |

Les systèmes communiquent par événements C# (`Action`) plutôt que par références directes.

## Lancer le projet

1. Ouvrir le dossier `PokerGame` avec **Unity 2022.3.10f1**.
2. Ouvrir la scène `Assets/Scenes/MainMenu.unity` et appuyer sur Play.
3. Pour un build mobile : *File > Build Settings*, choisir Android (SDK minimum 22).

## Pistes d'amélioration

- Écran de statistiques (commencé dans la scène `Statistique`).
- Prise en compte de l'as en tant que carte haute dans les quintes.
