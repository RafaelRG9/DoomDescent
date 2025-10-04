# Game Design Document: C# Fantasy Roguelike

## 1. High-Concept Pitch

* A classic style turn-based fantasy roguelike RPG that focuses on tactical combat and a robust crafting system for character progression and endless replayability.

## 2. Core Gameplay Loop

* **Explore** the dungeon.
* **Encounter** monsters.
* **Fight** in turn-based combat.
* **Collect** loot and crafting materials.
* **Craft** new gear at safe locations.
* **Explore Deeper** into more dangerous levels.
* **Inevitably Die** and learn from the run.
* **Return** to the main menu to start a new, fresh run.

## 3. Key Features

* [x] **Turn-Based Combat System**
  * [x] Stat-based damage (Strength, Defense).
  * [x] Dexterity-based miss chance.
  * [x] Color-coded feedback.
* [x] **Character Stats & Leveling**
  * [x] Experience gain from combat.
  * [x] Stat increases on level-up.
  * [x] `stats` command to view character sheet.
* [x] **Player Inventory & Equipment Slots**
  * [x] `inventory` and `examine` commands.
  * [x] `equip` and `unequip` logic.
  * [x] Dictionary-based equipment slots.
* [x] **Item & Crafting System**
  * [x] Inheritance for item types (`Weapon`, `Armor`, `Potion`).
  * [x] `use` command for consumables.
  * [x] Recipe-based crafting in designated safe areas.

* [ ] **Advanced Procedural Generation**
  * [ ] Create branching, interconnected dungeons (Improved Drunkard's Walk).
  * [ ] Add special room types (e.g., a guaranteed safe Start Room, a Boss Room).
* [ ] **Core Roguelike Loop**
  * [ ] A `Game` class to manage the entire session.
  * [ ] Game Over screen on player death.
  * [ ] A "Play Again?" option that generates a new player and a new dungeon.
  * [ ] Difficulty scaling (Dungeon Floors).
* [ ] **Enhanced User Experience**
  * [ ] Player Map system to show discovered rooms.
  * [ ] Atmospheric pacing with delayed text printing.

## 4. Core Data Models (Class Blueprint)

* **`Game`:** (A new class to manage the entire game session, including the main loop, player state, and dungeon instance.)
* **`Character`:** (Base class for players and monsters.)
* **`Player`:** (Inherits from `Character`.)
* **`Monster`:** (Inherits from `Character`.)
  * **`Goblin`:** (Inherits from `Monster`.)
  * **`Orc`:** (Inherits from `Monster`.)
* **`Item`:** (Base class for all items.)
  * **`Weapon`:** (Inherits from `Item`.)
  * **`Armor`:** (Inherits from `Item`.)
  * **`Potion`:** (Inherits from `Item`.)
* **`CraftingRecipe`:** (Contains required materials and resulting item.)
* **`Room`:** (A single location in the dungeon grid.)
* **`DungeonGenerator`:** (Responsible for creating the `Room` layout.)

## 5. Minimum Viable Product (MVP)

* [x] Player can fight monsters, collect loot, and craft in a post-combat safe area. *(Status: Completed)*
