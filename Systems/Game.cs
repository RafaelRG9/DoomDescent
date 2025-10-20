namespace csharp_roguelike_rpg.Systems;

using System.Net.Quic;
using csharp_roguelike_rpg.Abilities;
using csharp_roguelike_rpg.Characters;
using csharp_roguelike_rpg.Items;
using csharp_roguelike_rpg.World;

public class Game
{
    // --- FIELDS ---
    private Player _player = null!;
    private DungeonGenerator _generator;
    private GameData _gameData;
    private int _currentFloor;
    private Random _random;

    public Game()
    {
        _gameData = new GameData();
        _generator = new DungeonGenerator(_gameData);
        _random = new Random();
    }

    private void SetupNewGame()
    {
        _currentFloor = 1;
        Console.Clear();
        UIManager.SlowPrint("WELCOME TO YOUR DOOM!");

        string? playerName;
        do
        {
            UIManager.SlowPrint("What is your name, fool?");
            Console.Write("> ");
            playerName = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(playerName));

        UIManager.SlowPrint("Choose your weakling:");
        UIManager.SlowPrint("1. Warrior (High Health, High Strength)");
        UIManager.SlowPrint("2. Rogue   (High Dexterity, Balanced)");
        UIManager.SlowPrint("3. Mage    (Low Health, High Potential)");

        PlayerClass chosenClass;
        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();
            if (input == "1" || input?.ToLower() == "warrior")
            {
                chosenClass = PlayerClass.Warrior;
                break;
            }
            if (input == "2" || input?.ToLower() == "rogue")
            {
                chosenClass = PlayerClass.Rogue;
                break;
            }
            if (input == "3" || input?.ToLower() == "mage")
            {
                chosenClass = PlayerClass.Mage;
                break;
            }
            Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
        }
        UIManager.SlowPrint($"So, you will be playing the {chosenClass}? Good luck, you'll need it...");

        // --- WORLD AND PLAYER CREATION ---
        Room startingRoom = _generator.Generate(10, _currentFloor);
        _player = new Player(playerName, chosenClass);
        _player.CurrentRoom = startingRoom;
        _player.VisitedRooms.Add(startingRoom);
    }

    private void SetupNextFloor()
    {
        _currentFloor++;
        UIManager.SlowPrint($"You descend to floor {_currentFloor}...", ConsoleColor.Magenta);

        // Re-generate the world for the new floor
        Room startingRoom = _generator.Generate(10, _currentFloor);
        _player.CurrentRoom = startingRoom;
        _player.VisitedRooms.Clear(); // Clear the map for the new floor
        _player.VisitedRooms.Add(startingRoom);
    }

    private void HandlePlayerDeath()
    {
        UIManager.SlowPrint("\nYou have fallen in the dark...", ConsoleColor.Red);
        UIManager.SlowPrint("Your adventure ends here.", ConsoleColor.Red);
    }


    private bool GameLoop()
    {
        while (true)
        {
            Console.Write("\n> ");
            string? choice = Console.ReadLine();

            if (string.IsNullOrEmpty(choice))
            {
                UIManager.SlowPrint("Please enter a command.");
                continue;
            }

            // Split the input into words to get the command verb
            string[] words = choice.ToLower().Split(' ');
            string verb = words[0];

            // The new command parser
            switch (verb)
            {
                case "north":
                case "south":
                case "east":
                case "west":
                    if (_player.CurrentRoom != null)
                    {
                        CombatResult combatResult = CombatResult.PlayerVictory;
                        switch (verb)
                        {
                            case "north":
                                combatResult = MovePlayer(_player.CurrentRoom.North);
                                break;
                            case "south":
                                combatResult = MovePlayer(_player.CurrentRoom.South);
                                break;
                            case "east":
                                combatResult = MovePlayer(_player.CurrentRoom.East);
                                break;
                            case "west":
                                combatResult = MovePlayer(_player.CurrentRoom.West);
                                break;
                        }
                        if (combatResult == CombatResult.PlayerDefeat || combatResult == CombatResult.PlayerVictoryAndGameWon)
                        {
                            return false; // Exit GameLoop if player died or won
                        }
                    }
                    break;

                case "look":
                case "l":
                    DescribeRoom();
                    break;

                case "stats":
                case "char":
                    Console.WriteLine("\n--- Character Stats ---");
                    Console.WriteLine($" Name: {_player.Name}");
                    Console.WriteLine($" Class: {_player.Class}");
                    Console.WriteLine($" Level: {_player.Level}");
                    Console.WriteLine($" Experience: {_player.Experience} / {_player.ExperienceToNextLevel}");
                    Console.WriteLine($" Health: {_player.Health} / {_player.MaxHealth}");
                    Console.WriteLine($" Energy: {_player.Energy} / {_player.MaxEnergy}");
                    Console.WriteLine("-----------------------");
                    Console.WriteLine(" Attributes:");
                    foreach (var stat in _player.Stats)
                    {
                        Console.WriteLine($" - {stat.Key}: {stat.Value}");
                    }
                    Console.WriteLine(" Abilities:");
                    foreach (var ability in _player.Abilities)
                    {
                        Console.WriteLine($" - {ability.Name}(Cost: {ability.EnergyCost}): {ability.Description}");
                    }
                    Console.WriteLine("-----------------------");
                    break;

                case "map":
                case "m":
                    DrawMap();
                    break;

                case "inventory":
                case "i":
                    // display player inventory
                    if (_player.Inventory.Any())
                    {
                        Console.WriteLine("\nInventory");
                        foreach (var item in _player.Inventory)
                        {
                            Console.WriteLine($"- {item.Name}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nYour inventory is empty");
                    }
                    break;

                case "examine":
                case "x":
                    if (words.Length > 1)
                    {
                        string itemToExamineName = string.Join(" ", words.Skip(1));
                        Item? itemToExamine = _player.Inventory.Find(item => item.Name.Equals(itemToExamineName, StringComparison.OrdinalIgnoreCase));

                        if (itemToExamine != null)
                        {
                            Console.WriteLine($"\n{itemToExamine.Name}");
                            UIManager.SlowPrint($"  {itemToExamine.Description}");
                        }
                        else
                        {
                            Console.WriteLine($"You don't have a '{itemToExamineName}' in your inventory.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("What do you want to examine?");
                    }
                    break;

                case "take":
                    if (_player.CurrentRoom == null)
                    {
                        Console.WriteLine("Did you write anything?");
                        break;
                    }
                    if (words.Length > 1)
                    {
                        string itemToTakeName = string.Join(" ", words.Skip(1));
                        Item? itemToTake = _player.CurrentRoom.ItemsInRoom.Find(item => item.Name.Equals(itemToTakeName, StringComparison.OrdinalIgnoreCase));

                        if (itemToTake != null)
                        {
                            _player.Inventory.Add(itemToTake);
                            Console.WriteLine($"You took the {itemToTake.Name}");
                            _player.CurrentRoom.ItemsInRoom.Remove(itemToTake);
                        }
                    }
                    break;

                case "drop":
                    if (_player.CurrentRoom == null)
                    {
                        Console.WriteLine("STOP TRYING TO BREAK MY GAME! =(");
                        break;
                    }
                    if (words.Length > 1)
                    {
                        string itemToDropName = string.Join(" ", words.Skip(1));
                        Item? itemToDrop = _player.Inventory.Find(item => item.Name.Equals(itemToDropName, StringComparison.OrdinalIgnoreCase));

                        if (itemToDrop != null)
                        {
                            _player.Inventory.Remove(itemToDrop);
                            Console.WriteLine($"You dropped the {itemToDrop.Name}");
                            _player.CurrentRoom.ItemsInRoom.Add(itemToDrop);
                        }
                    }
                    break;

                case "equip":
                    if (words.Length > 1)
                    {
                        string itemToEquipName = string.Join(" ", words.Skip(1));

                        Item? itemToEquip = _player.Inventory.Find(item => item.Name.Equals(itemToEquipName, StringComparison.OrdinalIgnoreCase));

                        // Find if the item is in the Inventory
                        if (itemToEquip == null)
                        {
                            Console.WriteLine("You don't have that item in your inventory.");
                            break;
                        }
                        // If item is a Weapon
                        if (itemToEquip is Weapon weapon)
                        {
                            // Unequip any equipped item in the slot
                            if (_player.Equipment.ContainsKey(EquipmentSlot.MainHand))
                            {
                                _player.Inventory.Add(_player.Equipment[EquipmentSlot.MainHand]);
                                Console.WriteLine($"You unequip {_player.Equipment[EquipmentSlot.MainHand].Name}.");
                            }

                            // Equip the new weapon
                            _player.Equipment[EquipmentSlot.MainHand] = weapon;
                            _player.Inventory.Remove(weapon); // This line removes it from the inventory
                            Console.WriteLine($"You equip the {weapon.Name}.");
                        }
                        // If item is Armor
                        else if (itemToEquip is Armor armor)
                        {
                            // Unequip any equipped item in the slot
                            if (_player.Equipment.ContainsKey(EquipmentSlot.Chest))
                            {
                                _player.Inventory.Add(_player.Equipment[EquipmentSlot.Chest]);
                                Console.WriteLine($"You unequip {_player.Equipment[EquipmentSlot.Chest].Name}.");
                            }

                            // Equip the new armor
                            _player.Equipment[EquipmentSlot.Chest] = armor;
                            _player.Inventory.Remove(armor); // This line removes it from the inventory
                            Console.WriteLine($"You equip the {armor.Name}.");
                        }
                        else
                        {
                            // The item is not equippable.
                            Console.WriteLine("You can't equip that type of item.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("What do you want to equip?");
                    }
                    break;
                case "use":
                    if (words.Length > 1)
                    {
                        string itemToUseName = string.Join(" ", words.Skip(1));

                        Item? itemToUse = _player.Inventory.Find(item => item.Name.Equals(itemToUseName, StringComparison.OrdinalIgnoreCase));

                        if (itemToUse == null)
                        {
                            Console.WriteLine("You don't have that item in your inventory.");
                            break;
                        }
                        if (itemToUse is Potion potion)
                        {
                            _player.Health += potion.HealthToRestore;
                            if (_player.Health > _player.MaxHealth)
                            {
                                _player.Health = _player.MaxHealth; // Cap health at max health
                            }
                            _player.Inventory.Remove(potion);
                            UIManager.SlowPrint($"You use the {potion.Name} and restore {potion.HealthToRestore} health. You now have {_player.Health}/{_player.MaxHealth} HP", ConsoleColor.Green);
                        }
                        else
                        {
                            Console.WriteLine("You can't use that item.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("What do you want to use?");
                    }
                    break;
                case "craft":
                    // First, check if the room is safe (no monsters).
                    if (_player.CurrentRoom != null && _player.CurrentRoom.MonstersInRoom.Any())
                    {
                        UIManager.SlowPrint("It's too dangerous to craft here!", ConsoleColor.Red);
                        break;
                    }

                    // --- Enter the Crafting Sub-Menu ---
                    UIManager.SlowPrint("You approach a quiet corner and lay out your materials...", ConsoleColor.DarkGray);
                    while (true)
                    {
                        Console.WriteLine("\n--- Crafting Menu ---");
                        if (_gameData.AvailableRecipes.Any())
                        {
                            Console.WriteLine("Available recipes:");
                            foreach (var recipe in _gameData.AvailableRecipes)
                            {
                                Console.WriteLine($"- craft {recipe.ResultingItem.Name.ToLower()}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("You don't know any recipes.");
                        }
                        Console.WriteLine("- i (view inventory)");
                        Console.WriteLine("- leave");
                        Console.Write("> ");
                        string? craftingChoice = Console.ReadLine();

                        if (string.IsNullOrEmpty(craftingChoice)) continue;

                        if (craftingChoice.Equals("leave", StringComparison.OrdinalIgnoreCase))
                        {
                            UIManager.SlowPrint("You pack up your materials.");
                            break; // Exit the crafting loop
                        }
                        else if (craftingChoice.Equals("i", StringComparison.OrdinalIgnoreCase) || craftingChoice.Equals("inventory", StringComparison.OrdinalIgnoreCase))
                        {
                            if (_player.Inventory.Any())
                            {
                                Console.WriteLine("\nInventory");
                                foreach (var item in _player.Inventory)
                                {
                                    Console.WriteLine($"- {item.Name}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nYour inventory is empty");
                            }
                        }
                        else if (craftingChoice.StartsWith("craft ", StringComparison.OrdinalIgnoreCase))
                        {
                            string itemToCraftName = craftingChoice.Substring(6);

                            CraftingRecipe? recipe = _gameData.AvailableRecipes.Find(r => r.ResultingItem.Name.Equals(itemToCraftName, StringComparison.OrdinalIgnoreCase));

                            if (recipe == null)
                            {
                                Console.WriteLine("You don't know how to craft that.");
                                continue;
                            }

                            bool canCraft = true;
                            foreach (var material in recipe.RequiredMaterials)
                            {
                                int materialCount = _player.Inventory.Count(item => item.Name == material.Key.Name);
                                if (materialCount < material.Value)
                                {
                                    Console.WriteLine($"You don't have enough {material.Key.Name}. Required: {material.Value}, Have: {materialCount}");
                                    canCraft = false;
                                    break;
                                }
                            }

                            if (canCraft)
                            {
                                foreach (var material in recipe.RequiredMaterials)
                                {
                                    for (int i = 0; i < material.Value; i++)
                                    {
                                        Item itemToRemove = _player.Inventory.First(item => item.Name == material.Key.Name);
                                        _player.Inventory.Remove(itemToRemove);
                                    }
                                }
                                _player.Inventory.Add(recipe.ResultingItem);
                                UIManager.SlowPrint($"Successfully crafted {recipe.ResultingItem.Name}!", ConsoleColor.Green);
                            }
                        }
                    }
                    break;
                case "descend":
                case "down":
                    if (_gameData.StairsDown != null &&
                        _player.CurrentRoom != null &&
                        _player.CurrentRoom.ItemsInRoom.Contains(_gameData.StairsDown))
                    {
                        return true;
                    }
                    else
                    {
                        UIManager.SlowPrint("There is nowhere to descend to here.");
                    }
                    break;
                case "drink":
                    // Check if the player is in a Fountain Room
                    if (_player.CurrentRoom?.Name != "Fountain Room")
                    {
                        UIManager.SlowPrint("There is nothing to drink here.");
                        break;
                    }
                    // Get two random blessings from the Blessings table
                    var blessings = _gameData.FountainBlessings.OrderBy(x => _random.Next()).Take(2).ToList();

                    UIManager.SlowPrint("You approach the glowing fountain. What do you do?", ConsoleColor.Cyan);
                    Console.WriteLine($"1. {blessings[0].Description}");
                    Console.WriteLine($"2. {blessings[1].Description}");
                    Console.WriteLine("3. Drink deeply to restore your health.");

                    // Process player choices and apply appropriate blessing or full heal
                    while (true)
                    {
                        Console.Write("> ");
                        string? input = Console.ReadLine();
                        if (input == "1")
                        {
                            blessings[0].Apply(_player);
                            break;
                        }
                        if (input == "2")
                        {
                            blessings[1].Apply(_player);
                            break;
                        }
                        if (input == "3")
                        {
                            _player.Health = _player.MaxHealth;
                            UIManager.SlowPrint("The water invigorates you, restoring your health to full!", ConsoleColor.Green);
                            break;
                        }
                        Console.WriteLine("Invalid choice.");
                    }

                    // Fountain is meant to be used once, make inert after use
                    _player.CurrentRoom.Name = "Inert Fountain Room";
                    _player.CurrentRoom.Description = "The fountain's glow has faded, its magic spent.";
                    break;
                case "quit":
                    UIManager.SlowPrint("You decide to rest for now. Until next time!");
                    return false;

                default:
                    Console.WriteLine("I don't understand that command.");
                    break;
            }
        }
    }
    public void Run()
    {
        // The master "Play Again?" loop
        while (true)
        {
            SetupNewGame(); // Creates a new character on floor 1

            // The "Dungeon Floor" loop for a single run
            bool playerIsAlive = true;
            while (playerIsAlive)
            {
                DescribeRoom();
                bool shouldDescend = GameLoop();

                if (shouldDescend) // Player wants to descend
                {
                    _currentFloor++;
                    SetupNextFloor();
                }
                else // Player quit, died, or won the game
                {
                    playerIsAlive = false; // Exit the floor loop
                }

                // --- AFTER RUN IS OVER ---
                if (_player.Health <= 0)
                {
                    HandlePlayerDeath();
                }

                UIManager.SlowPrint("\nPlay Again? (y/n)");
                string? playAgain = Console.ReadLine();
                if (playAgain?.ToLower() != "y")
                {
                    break; // Exit the master "Play Again?" loop
                }
            }
            UIManager.SlowPrint("Thanks for playing!");
        }
    }

    //----------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------

    // --- HELPER METHODS ---

    private CombatResult StartCombat(Monster monsterToFight)
    {
        Random random = new Random();

        UIManager.SlowPrint($"{_player.Name} encounters a fierce {monsterToFight.Name}");

        UIManager.SlowPrint("Rolling for initiave... feeling lucky?");
        var combatants = new List<Character> { _player, monsterToFight };
        var turnOrder = combatants.OrderByDescending(c => c.Stats["Dexterity"] + random.Next(1, 21)).ToList();
        UIManager.SlowPrint("Turn order is set!");
        foreach (var character in turnOrder)
        {
            UIManager.SlowPrint($"- {character.Name}", ConsoleColor.DarkGray);
        }



        // Turn-Based combat
        int round = 1;
        while (_player.Health > 0 && monsterToFight.Health > 0)
        {
            // Display Status
            Console.WriteLine("\n--------------------");
            Console.WriteLine($"{_player.Name}: {_player.Health}/{_player.MaxHealth}");
            Console.WriteLine($"Energy {_player.Energy}/{_player.MaxEnergy}");
            Console.WriteLine(" ");
            Console.WriteLine($"{monsterToFight.Name}: {monsterToFight.Health}/{monsterToFight.MaxHealth}");
            Console.WriteLine($"Energy: {monsterToFight.Energy}/{monsterToFight.MaxEnergy}");
            Console.WriteLine("\n--------------------");

            Console.WriteLine($"\n--- ROUND {round} ---");

            foreach (var character in turnOrder)
            {
                character.IsDefending = false;

                if (character.Health <= 0) continue;
                if (_player.Health <= 0 || monsterToFight.Health <= 0) break;

                if (character is Player)
                {
                    _player.Energy += 5;
                    if (_player.Energy == _player.MaxEnergy)
                    {
                        _player.Energy = _player.MaxEnergy;
                    }
                    UIManager.SlowPrint("Your turn! Choose an action");
                    Console.WriteLine("- attack");
                    Console.WriteLine("- defend");
                    foreach (var ability in _player.Abilities)
                    {
                        Console.WriteLine($"- {ability.Name?.ToLower()} (Cost: {ability.EnergyCost})");
                    }
                    Console.Write("> ");
                    string? playerChoice = Console.ReadLine();

                    // Safety check to prevent passing null values
                    if (string.IsNullOrEmpty(playerChoice))
                    {
                        Console.WriteLine("You must enter a command!");
                        continue; // Skip the rest of the turn
                    }

                    // Process Player's Action
                    if (playerChoice.Equals("attack", StringComparison.OrdinalIgnoreCase))
                    {
                        int attackRoll = random.Next(1, 21) + _player.Stats["Dexterity"];
                        int evasionClass = 10 + monsterToFight.Stats["Dexterity"];

                        if (attackRoll >= evasionClass)
                        {
                            // Calculate damage based on player strength and weapon
                            int damageDealt = _player.GetTotalStrength();
                            monsterToFight.Health -= damageDealt;
                            UIManager.SlowPrint($"You attack the {monsterToFight.Name}, dealing {damageDealt} damage!", ConsoleColor.Cyan);

                            // Apply lifesteal if applicable
                            if (_player.Stats["Lifesteal"] > 0)
                            {
                                int lifestealAmount = _player.Stats["Lifesteal"] * damageDealt / 100;
                                _player.Health += lifestealAmount;
                                if (_player.Health > _player.MaxHealth)
                                {
                                    _player.Health = _player.MaxHealth; // Cap health at max health
                                }
                                UIManager.SlowPrint($"You steal {lifestealAmount} health from the {monsterToFight.Name}!", ConsoleColor.Green);
                            }
                        }
                        else
                        {
                            UIManager.SlowPrint("You swing and miss!", ConsoleColor.Gray);
                        }
                    }
                    else if (playerChoice.Equals("defend", StringComparison.OrdinalIgnoreCase))
                    {
                        _player.IsDefending = true;
                        UIManager.SlowPrint($"{_player.Name} takes a defensive stance!", ConsoleColor.White);
                    }
                    else
                    {
                        Ability? chosenAbility = _player.Abilities.Find(a => a.Name?.Equals(playerChoice, StringComparison.OrdinalIgnoreCase) ?? false);
                        if (chosenAbility != null)
                        {
                            if (_player.Energy >= chosenAbility.EnergyCost)
                            {
                                chosenAbility.Use(_player, monsterToFight);
                                _player.Energy -= chosenAbility.EnergyCost;
                            }
                            else
                            {
                                UIManager.SlowPrint("Fool, you do not have anough energy, can't you read?");
                            }
                        }
                        else
                        {
                            UIManager.SlowPrint("Invalid command! You hesitate and lose your turn.");
                        }
                    }
                    Console.WriteLine("\n--------------------");
                    Console.WriteLine($"{_player.Name}: {_player.Health}/{_player.MaxHealth}");
                    Console.WriteLine($"Energy {_player.Energy}/{_player.MaxEnergy}");
                    Console.WriteLine(" ");
                    Console.WriteLine($"{monsterToFight.Name}: {monsterToFight.Health}/{monsterToFight.MaxHealth}");
                    Console.WriteLine($"Energy: {monsterToFight.Energy}/{monsterToFight.MaxEnergy}");
                    Console.WriteLine("\n--------------------");
                }

                else if (character is Monster actingMonster)
                {
                    // --- NEW TO-HIT LOGIC FOR MONSTER ---
                    int attackRoll = random.Next(1, 21) + actingMonster.Stats["Dexterity"];
                    int evasionClass = 10 + _player.Stats["Dexterity"];

                    if (attackRoll >= evasionClass)
                    {
                        // It's a hit!
                        int monsterDamage = actingMonster.Stats["Strength"];
                        int playerDefense = _player.GetTotalDefense();
                        int damageToPlayer = monsterDamage - playerDefense;

                        if (_player.IsDefending)
                        {
                            damageToPlayer /= 2;
                            UIManager.SlowPrint($"{_player.Name} braces for the hit!", ConsoleColor.White);
                        }

                        if (damageToPlayer < 1)
                        {
                            damageToPlayer = 1;
                        }

                        _player.Health -= damageToPlayer;
                        UIManager.SlowPrint($"The {actingMonster.Name} attacks, dealing {damageToPlayer} damage to you!", ConsoleColor.Red);
                    }
                    else
                    {
                        // It's a miss.
                        UIManager.SlowPrint($"The {actingMonster.Name} attacks and misses!", ConsoleColor.Gray);
                    }
                }
                round++;
                UIManager.SlowPrint($"\n---------END OF ROUND {round}-----------");
                UIManager.SlowPrint($"{_player.Name}: {_player.Health}/{_player.MaxHealth}");
                UIManager.SlowPrint($"Energy: {_player.Energy}/{_player.MaxEnergy}");
                UIManager.SlowPrint($"{monsterToFight.Name}: {monsterToFight.Health}/{monsterToFight.MaxHealth}");
                UIManager.SlowPrint($"Energy: {monsterToFight.Energy}/{monsterToFight.MaxEnergy}");
                UIManager.SlowPrint("\n----------------------------------");
            }
        }

        Console.WriteLine("\n--- BATTLE OVER ---");

        // Check for winners
        if (_player.Health > 0)
        {
            UIManager.SlowPrint($"You defeated the {monsterToFight.Name}!");

            if (monsterToFight is Boss)
            {
                // Check if this is the FINAL floor
                if (_currentFloor == 5)
                {
                    UIManager.SlowPrint("You have defeated the final boss!", ConsoleColor.Yellow);
                    UIManager.SlowPrint("The dungeon crumbles... you are victorious!", ConsoleColor.Yellow);
                    UIManager.SlowPrint("For now...", ConsoleColor.Red);
                    return CombatResult.PlayerVictoryAndGameWon; // Send the "Game Won" signal
                }
                else // It's a regular floor boss, spawn the stairs
                {
                    if (_player.CurrentRoom != null && _gameData.StairsDown != null)
                    {
                        UIManager.SlowPrint("A staircase reveals itself!", ConsoleColor.Yellow);
                        _player.CurrentRoom.ItemsInRoom.Add(_gameData.StairsDown);
                    }
                }
            }
            if (monsterToFight.Loot != null)
            {
                UIManager.SlowPrint($"You found a {monsterToFight.Loot.Name}!");
                _player.Inventory.Add(monsterToFight.Loot);
            }

            // Calculate experience and level up
            bool leveledUp = _player.AddExperience(monsterToFight.ExperienceValue);
            if (leveledUp)
            {
                UIManager.SlowPrint($"\n*** You have reached Level {_player.Level}! ***", ConsoleColor.Yellow);
                HandleTalentSelection();
            }
            return CombatResult.PlayerVictory;
        }
        else
        {
            return CombatResult.PlayerDefeat;
        }
    }

    private void DescribeRoom()
    {
        if (_player.CurrentRoom == null)
        {
            Console.WriteLine("You are lost in a void...");
            return;
        }

        UIManager.SlowPrint($"\nLocation: {_player.CurrentRoom.Name} ({_player.CurrentRoom.X}, {_player.CurrentRoom.Y})");
        UIManager.SlowPrint(_player.CurrentRoom.Description);

        // --- MONSTER OR ITEM LIST ---
        if (_player.CurrentRoom.MonstersInRoom.Any())
        {
            UIManager.SlowPrint("\nDangerous creatures lurk here:");
            foreach (Monster monster in _player.CurrentRoom.MonstersInRoom)
            {
                UIManager.SlowPrint($"- A fearsome {monster.Name}", ConsoleColor.Red);
            }
        }
        else
        {
            if (_player.CurrentRoom.ItemsInRoom.Any())
            {
                UIManager.SlowPrint("\nYou found treasure in this room:");
                foreach (Item item in _player.CurrentRoom.ItemsInRoom)
                {
                    UIManager.SlowPrint($"- {item.Name}", ConsoleColor.Green);
                }
            }
        }
    }

    private CombatResult MovePlayer(Room? newRoom)
    {
        if (newRoom != null)
        {
            // Check for boss room and confirm
            if (newRoom.Name == "Boss Chamber")
            {
                UIManager.SlowPrint("You sense a foul stench coming from the other side of this door...", ConsoleColor.Red);
                UIManager.SlowPrint("There is a boss on the other side", ConsoleColor.Red);
                UIManager.SlowPrint("Do you want to proceed?", ConsoleColor.Red);
                Console.Write("(y/n)> ");
                string? input = Console.ReadLine();
                if (input?.ToLower() != "y")
                {
                    UIManager.SlowPrint("You decide to stay back and prepare yourself...", ConsoleColor.Yellow);
                    return CombatResult.InProgress;
                }
                else
                {
                    UIManager.SlowPrint("You bravely step into the chamber...", ConsoleColor.Yellow);
                }
            }

            // Move the player to the new room
            _player.CurrentRoom = newRoom;
            _player.VisitedRooms.Add(newRoom);
            DescribeRoom();

            // Trigger combat if it is a monster room
            while (_player.CurrentRoom != null &&
               _player.CurrentRoom.MonstersInRoom.Count > 0 &&
               _player.Health > 0)
            {
                Monster monster = _player.CurrentRoom.MonstersInRoom[0];
                CombatResult combatResult = StartCombat(monster);

                if (combatResult == CombatResult.PlayerVictory)
                {
                    _player.CurrentRoom.MonstersInRoom.Remove(monster);
                }
                else
                {
                    // If player was defeated OR won the whole game, signal back
                    return combatResult;
                }
            }
        }
        else
        {
            Console.WriteLine("You can't go that way.");
        }
        return CombatResult.PlayerVictory;
    }

    private void DrawMap()
    {
        // null guard
        if (_player.CurrentRoom == null)
        {
            Console.WriteLine("Player is not in the room, how did you do that?");
            return; // Exit the method early
        }

        // Find the boundaries of the discovered map.
        int minX = _player.VisitedRooms.Min(r => r.X);
        int maxX = _player.VisitedRooms.Max(r => r.X);
        int minY = _player.VisitedRooms.Min(r => r.Y);
        int maxY = _player.VisitedRooms.Max(r => r.Y);

        Console.WriteLine("\n--- Dungeon Map ---");

        // Loop through the grid from top to bottom.
        for (int y = maxY; y >= minY; y--)
        {
            // A string for each row of the map.
            string line = "";
            for (int x = minX; x <= maxX; x++)
            {
                // Check if the player is at the current coordinate.
                if (_player.CurrentRoom.X == x && _player.CurrentRoom.Y == y)
                {
                    line += "[P]"; // Player's position
                }
                // Check if a visited room exists at the current coordinate.
                else if (_player.VisitedRooms.Any(r => r.X == x && r.Y == y))
                {
                    if (_player.VisitedRooms.First(r => r.X == x && r.Y == y).MonstersInRoom.Any())
                    {
                        line += "[M]"; // Room with monsters
                    }
                    else if (_player.VisitedRooms.First(r => r.X == x && r.Y == y).ItemsInRoom.Any())
                    {
                        line += "[T]"; // Room with items
                    }
                    else if (_player.VisitedRooms.First(r => r.X == x && r.Y == y).Name == "Boss Chamber")
                    {
                        line += "[B]"; // Boss room
                    }
                    else
                    {
                        line += "[#]"; // Visited room
                    }
                }
                else
                {
                    line += "   "; // Empty, undiscovered space
                }
            }
            Console.WriteLine(line);
        }
        Console.WriteLine("-------------------");
        Console.WriteLine("Legend: P=Player, M=Monsters, T=Treasure, B=Boss");
    }

    private void HandleTalentSelection()
    {
        // Check if there are any talents defined for the player's current class and level.
        if (_gameData.ClassTalents.ContainsKey(_player.Class) &&
            _gameData.ClassTalents[_player.Class].ContainsKey(_player.Level))
        {
            UIManager.SlowPrint("You have a new talent point to spend!", ConsoleColor.Magenta);

            // Get the list of available talents for this tier.
            var availableTalents = _gameData.ClassTalents[_player.Class][_player.Level];

            while (true)
            {
                UIManager.SlowPrint("Choose your talent:", ConsoleColor.Magenta);
                // Display the options
                for (int i = 0; i < availableTalents.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {availableTalents[i].Name} - {availableTalents[i].Description}");
                }

                Console.Write("> ");
                string? input = Console.ReadLine();

                // Try to parse the input as a number
                if (int.TryParse(input, out int choice) && choice > 0 && choice <= availableTalents.Count)
                {
                    // Get the chosen modifier
                    Modifier chosenTalent = availableTalents[choice - 1];

                    // Apply it to the player
                    chosenTalent.Apply(_player);
                    break; // Exit the selection loop
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
            }
        }
    }
}