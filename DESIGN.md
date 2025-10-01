# Game Design Document: C# Fantasy Roguelike

## 1. High-Concept Pitch

* A classic style turn-based fantasy roguelite RPG that focuseson tactical combat and a robust crafting system for character progression and endless replayability

## 2. Core Gameplay Loop

* Explore the dungeon
* Encounter monsters
* Fight in turn-based combat
* Collect loot and crafting material
* Return to a safe location
* Craft new weapons and items from materials
* Repeat by exploring deeper into the dungeon

## 3. Key Features

* [ ] Turn-Based Combat System
* [ ] Character Stats & Leveling
* [ ] Player Inventory & Equipment Slots
* [ ] Item & Crafting System
* [ ] Procedurally Generated Dungeon Maps

## 4. Core Data Models (Class Blueprint)

* **Character:** (Base class for players and monsters. Will have stats, health, etc.)
* **Player:** (Inherits from `Character`. Will also have experience, level.)
* **Monster:** (Inherits from `Character`. Will have loot tables, AI behavior, and level but no experience)
* **Item:** (Base class for all items.)
* **Weapon:** (Inherits from `Item`. Will have a damage property.)
* **Armor:** (Inherits from `Item`. Will have a defense property.)
* **Potions:** (Inherits from `Item`. Will have a effect property.)
* **CraftingRecipe:** (Will contain required materials and the resulting item.)

## 5. Minimum Viable Product (MVP)

* Player can fight monsters in a single room, collect dropped items and craft in a separate safe room.