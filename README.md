# DoomDescent: A C# Procedural Roguelike

**DoomDescent** is a feature-rich, turn-based fantasy roguelike built from the ground up in C#. It is a portfolio project designed to showcase understanding of professional software architecture, object-oriented design, and complex, interlocking game systems.

The player chooses a class, explores procedurally generated dungeon floors, and fights monsters in tactical, turn-based combat. The goal is to grow powerful enough to defeat the bosses of all five floors and vanquish the final BBEG.

## Key Features

* **Three Unique Player Classes:** Choose from the **Warrior**, **Rogue**, or **Mage**, each with unique starting stats, class-specific abilities, and custom level-up stat growth.
* **Deep Progression Systems:**
    * **Class-Specific Talents:** At levels 3, 6, and 9, choose one of three unique talents to permanently power up your character.
    * **Fountain Blessings:** Discover rare fountain rooms that offer a choice of powerful, permanent stat boosts or a full heal.
    * **Boss Relics:** Defeat floor bosses to earn unique Relics that grant game-changing effects, like Lifesteal or new abilities.
* **Tactical Turn-Based Combat:**
    * **Initiative System:** Turn order is dynamically determined at the start of combat based on a Dexterity + d20 roll.
    * **Strategic Choices:** `attack`, `defend` to halve incoming damage, or use powerful, resource-costing class **Abilities**.
    * **Contested Rolls:** Attacks are resolved by the attacker's roll against the defender's Evasion Class, making Dexterity a vital stat for both offense and defense.
* **Procedural Dungeon Floors:**
    * **Infinite Dungeons:** Each floor is procedurally generated using a modified "Drunkard's Walk" algorithm to create branching, non-linear layouts.
    * **Difficulty Scaling:** Monsters and bosses grow stronger as you descend to deeper floors.
    * **Dynamic Rooms:** Explore monster lairs, treasure rooms, and rare fountain rooms, with descriptions that update as you clear them.
* **Crafting & Equipment:** Clear rooms of monsters to create a safe space to craft. Use materials dropped by enemies to forge new, more powerful weapons and armor.

---

## How to Play

### 1. Download the Game (Recommended)
The easiest way to play is to download the latest packaged `.exe` from the **Releases** page. No installation is required.

https://github.com/RafaelRG9/DoomDescent/releases/tag/v1.0.0

### 2. Build from Source
If you prefer, you can clone the repository and run the game using the .NET SDK.
1.  Clone the repo: `git clone https://github.com/RafaelRG9/DoomDescent.git`
2.  Navigate to the directory: `cd DoomDescent`
3.  Run the game: `dotnet run`

---

## Gameplay Commands

* **Movement:** `north`, `south`, `east`, `west`
* **Actions:** `look`, `take [item]`, `drop [item]`, `equip [item]`, `use [item]`, `inspect [item]` 
* **Character:** `stats`, `inventory`, `map`
* **Combat:** `attack`, `defend`, `[ability name]`
* **Other:** `craft`, `drink`, `descend`

---

## Technical Deep Dive

This project was built from scratch to demonstrate a scalable and maintainable C# architecture.

### 1. Architecture: Separation of Concerns
The project is built on a "Separation of Concerns" model:
* **`Game.cs` (The Engine):** The central system that manages the main game state, game loops (floor progression, "Play Again?"), and all helper methods. It acts as the "director" that coordinates all other systems.
* **`GameData.cs` (The Database):** A centralized static data class that is loaded on startup. It holds all game content—item definitions, monster templates, boss lists, crafting recipes, and the multi-layered Talent Tree dictionary. This separation allows for easy content expansion without ever touching the game's core logic.
* **`UIManager.cs` (The View):** A static utility class that handles all `Console` output. All `SlowPrint` and color-coded messages are managed here, ensuring that no other class is responsible for drawing to the screen.

### 2. Object-Oriented Design
* **Abstract Inheritance:** The project uses abstraction and inheritance. `Character` is the base for `Player` and `Monster`. `Monster` is the base for all specific monster types (`Goblin`, `Orc`, `Boss`). This allows for shared logic (like stats and health) while enabling specialized behavior.
* **The Prototype Pattern:** To create level-scaled monsters, the `DungeonGenerator` doesn't know about `Goblin`s or `Orc`s. It simply pulls a `Monster` template from `GameData`, calls `monster.Clone()`, and then calls `monster.ScaleStats(floorLevel)`. This makes the generator infinitely scalable—new monsters can be added to `GameData` and will appear in the game with no changes to the generator's code.
* **The Modifier System:** A powerful `abstract class Modifier` is used as a blueprint for all permanent character upgrades. `StatModifier` and `AbilityGrantModifier` are concrete implementations. This single system cleanly powers three different features: **Talents**, **Fountain Blessings**, and **Relics**.
