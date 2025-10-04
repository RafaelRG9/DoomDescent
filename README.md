# C# Procedural Roguelike RPG

A classic, turn-based fantasy roguelike built from the ground up in C#. This project focuses on demonstrating core game systems design, object-oriented principles, and procedural content generation, all within a polished console application.

## Key Features

* **Infinite Dungeons:** Explore a new, unique dungeon on every playthrough, procedurally generated using a "Drunkard's Walk" algorithm.
* **Tactical Turn-Based Combat:** Engage in classic RPG combat with mechanics for attacking, missing, and damage reduction based on stats and gear.
* **Character Progression:** Gain experience from defeating monsters to level up, increasing your stats and becoming more powerful.
* **Equipment System:** Find, equip, and manage `Weapons` and `Armor` in distinct equipment slots. Gear directly impacts combat performance.
* **Consumables & Crafting:** Use `Potions` to heal and gather materials from monsters to `craft` new items.

## Technical Deep Dive

This project was built to showcase a strong understanding of C# and core software engineering principles.

* **Architecture (OOP):** The project is built on a strong object-oriented foundation. A multi-layer inheritance hierarchy (`Character` -> `Monster` -> `Goblin`/`Orc`) is used to model game entities, promoting code reuse and scalability. Game logic is encapsulated into distinct classes (`DungeonGenerator`) and helper methods (`StartCombat`, `MovePlayer`) to follow the Single Responsibility Principle.

* **Procedural Generation:** The world is generated using a "Drunkard's Walk" algorithm. A `Dictionary<string, Room>` serves as a flexible and efficient sparse grid to store the map data, allowing for complex and non-rectangular dungeon layouts.

* **Game Systems Design:**
  * **Combat:** The turn-based combat system uses character stats (`Strength`, `Dexterity`) and equipped item properties (`Damage`, `Defense`) to calculate outcomes, including a miss chance based on a simulated "to-hit" roll.
  * **Items & Inheritance:** The item system uses inheritance (`Item` -> `Weapon`/`Armor`/`Potion`) and C#'s `is` keyword for type-checking, allowing for different item types with unique properties and behaviors (`equip` vs. `use`).
  * **State Management:** The main game loop is a state machine that cleanly transitions between exploration, combat, and crafting phases.

## How to Run

1. Clone the repository.
2. Navigate to the project directory.
3. Run the application with `dotnet run`.
