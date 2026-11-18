# Mobile Poker

*[Version française](README.fr.md)*

A five-card draw poker game played head-to-head against the computer, designed for mobile in landscape mode.
Solo academic project made with **Unity 2022.3** and **C#** (March – April 2024).

## How it plays

A match lasts **10 rounds**. Each round, the player picks the cards to hold, draws to replace the others, and their hand is compared with the opponent's. The winning hand scores points based on its ranking. Whoever has the most points after round 10 wins the match and earns gold.

Gold buys advantages in the shop, and the main menu lets the player customize their deck before a match.

## Features

- **Hand evaluation**: detects all 10 hand rankings, from high card to royal flush, and compares them with the opponent's hand to decide who wins the round.
- **Holding cards**: tap a card to keep it before drawing. The number of cards you can hold is limited and can be increased in the shop.
- **In-game shop**: an extra card to hold, double score for the round, double gold, and resetting the opponent's score.
- **Deck building**: from the main menu, the player removes cards from their deck and adds jokers. The black joker earns gold; the red joker takes 5 points from the opponent.
- **Persistence across scenes**: gold and the modified deck are stored in ScriptableObjects.
- **Card animations** for the player and the opponent, win and lose screens, and volume settings.

## Architecture

| Script | Role |
| --- | --- |
| `GameManager` | Game loop, drawing, hand evaluation, scoring and end of match |
| `Carte` / `CarteData` | Card display and touch selection (`IPointerDownHandler`) |
| `ShopManager` | In-game shop and gold |
| `MenuShop` | Deck building from the main menu |
| `CardAnimation` | Dealing animations |
| `SOScripts/*` | ScriptableObjects: sprites, saved deck, gold, audio |

Systems communicate through C# events (`Action`) rather than direct references.

## Running the project

1. Open the `PokerGame` folder with **Unity 2022.3.10f1**.
2. Open `Assets/Scenes/MainMenu.unity` and press Play.
3. For a mobile build: *File > Build Settings*, select Android (minimum SDK 22).

## Possible improvements

- Statistics screen (started in the `Statistique` scene).
- Treat the ace as a high card so that 10-J-Q-K-A straights are detected.
