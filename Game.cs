public class Game
{
    //Game's state variables.
    private Player _player = null!;
    private DungeonGenerator _generator;
    private List<CraftingRecipe> _availableRecipes;

    public Game()
    {
        _generator = new DungeonGenerator();
        _availableRecipes = new List<CraftingRecipe>();

        // --- ITEMS AND RECIPES ---
        Item goblinHide = new Item("Goblin Hide", "Yuck! What did you do to loot this? leathery and tough but, extremely smelly!");
        Item leatherArmor = new Item("Leather Armor", "Simple armor made from goblin hide, you couldn't get the smell off, nasty!");
        CraftingRecipe armorRecipe = new CraftingRecipe(leatherArmor, new Dictionary<Item, int>
        {
            { goblinHide, 2 }
        });
        _availableRecipes.Add(armorRecipe);
    }

    private void SetupNewGame()
    {
        Console.Clear();
        Console.WriteLine("Here we go!");

        // --- WORLD AND PLAYER CREATION ---
        Room startingRoom = _generator.Generate(10);
        _player = new Player("Hero", 50, 10, 5);
        _player.CurrentRoom = startingRoom;
        _player.VisitedRooms.Add(startingRoom);
    }


    private void GameLoop()
    {
        while (true)
        {
            Console.Write("\n> ");
            string? choice = Console.ReadLine();

            if (string.IsNullOrEmpty(choice))
            {
                Console.WriteLine("Please enter a command.");
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
                        switch (verb)
                        {
                            case "north":
                                MovePlayer(_player.CurrentRoom.North);
                                break;
                            case "south":
                                MovePlayer(_player.CurrentRoom.South);
                                break;
                            case "east":
                                MovePlayer(_player.CurrentRoom.East);
                                break;
                            case "west":
                                MovePlayer(_player.CurrentRoom.West);
                                break;
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
                    Console.WriteLine($" Level: {_player.Level}");
                    Console.WriteLine($" Experience: {_player.Experience} / {_player.ExperienceToNextLevel}");
                    Console.WriteLine($" Health: {_player.Health} / {_player.MaxHealth}");
                    Console.WriteLine("-----------------------");
                    Console.WriteLine(" Attributes:");
                    foreach (var stat in _player.Stats)
                    {
                        Console.WriteLine($" - {stat.Key}: {stat.Value}");
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
                            Console.WriteLine($"  {itemToExamine.Description}");
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
                            PrintInColor($"You use the {potion.Name} and restore {potion.HealthToRestore} health. You now have {_player.Health}/{_player.MaxHealth} HP", ConsoleColor.Green);
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
                case "quit":
                    Console.WriteLine("You decide to rest for now. Until next time!");
                    return; // Use 'return' to exit the application from the main context.

                default:
                    Console.WriteLine("I don't understand that command.");
                    break;
            }
        }
    }
    public void Run()
    {
        while (true)
        {
            SetupNewGame();
            DescribeRoom();
            GameLoop();

            Console.WriteLine("\nPlay Again? (y/n)");
            string? playAgain = Console.ReadLine();

            if (playAgain?.ToLower() != "y")
            {
                break;
            }
        }
        Console.WriteLine("Thanks for playing!");
    }
    // --- HELPER METHODS ---

    // Utility method to print text in a specific color
    private void PrintInColor(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    private bool StartCombat(Monster monsterToFight)
    {
        Random random = new Random();
        Console.WriteLine($"{_player.Name} encounters a fierce {monsterToFight.Name}");

        // Turn-Based combat
        while (_player.Health > 0 && monsterToFight.Health > 0)
        {
            // Display Status
            Console.WriteLine("\n--------------------");
            Console.WriteLine($"{_player.Name}: {_player.Health}/{_player.MaxHealth}");
            Console.WriteLine($"{monsterToFight.Name}: {monsterToFight.Health}/{monsterToFight.MaxHealth}");
            Console.WriteLine("\n--------------------");

            // Request Player for Action
            Console.WriteLine("Your turn! Type 'attack' to fight");
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
                if (random.Next(20) + _player.Stats["Dexterity"] > 10)
                {
                    // Calculate damage based on player strength and weapon
                    int damageDealt = _player.GetTotalStrength();
                    monsterToFight.Health -= damageDealt;
                    PrintInColor($"You attack the {monsterToFight.Name}, dealing {damageDealt} damage!", ConsoleColor.Cyan);
                }
                else
                {
                    PrintInColor("You swing and miss!", ConsoleColor.Gray);
                }
            }
            else
            {
                Console.WriteLine("Invalid command! You hesitate and lose your turn.");
            }

            // Check if Monster still lives and calculate their attack
            if (monsterToFight.Health > 0)
            {
                if (random.Next(20) + monsterToFight.Stats["Dexterity"] > 10)
                {
                    // Calculate damage to player based on monster strength and player defense
                    int monsterDamage = monsterToFight.Stats["Strength"];
                    int playerDefense = _player.GetTotalDefense();
                    int damageToPlayer = monsterDamage - playerDefense;

                    // Ensure at least 1 damage is dealt
                    if (damageToPlayer < 1)
                    {
                        damageToPlayer = 1;
                    }

                    // Deal damage to player
                    _player.Health -= damageToPlayer;
                    PrintInColor($"The {monsterToFight.Name} retaliates, dealing {damageToPlayer} damage to you!", ConsoleColor.Red);
                }
                else
                {
                    PrintInColor($"The {monsterToFight.Name} attacks and misses!", ConsoleColor.Gray);
                }
            }
        }

        Console.WriteLine("\n--- BATTLE OVER ---");

        // Check for winners
        if (_player.Health > 0)
        {
            Console.WriteLine($"You defeated the {monsterToFight.Name}!");
            if (monsterToFight.Loot != null)
            {
                Console.WriteLine($"You found a {monsterToFight.Loot.Name}!");
                _player.Inventory.Add(monsterToFight.Loot);
            }
            _player.AddExperience(monsterToFight.ExperienceValue);

            return true;
        }
        else
        {
            return false;
        }
    }

    private void DescribeRoom()
    {
        if (_player.CurrentRoom == null)
        {
            Console.WriteLine("You are lost in a void...");
            return;
        }

        Console.WriteLine($"\nLocation: {_player.CurrentRoom.Name} ({_player.CurrentRoom.X}, {_player.CurrentRoom.Y})");
        Console.WriteLine(_player.CurrentRoom.Description);

        // --- MONSTER OR ITEM LIST ---
        if (_player.CurrentRoom.MonstersInRoom.Count > 0)
        {
            Console.WriteLine("\nDangerous creatures lurk here:");
            foreach (Monster monster in _player.CurrentRoom.MonstersInRoom)
            {
                Console.WriteLine($"- A fearsome {monster.Name}");
            }
        }
        else
        {
            if (_player.CurrentRoom.ItemsInRoom.Count > 0)
            {
                Console.WriteLine("\nYou found treasure in this room:");
                foreach (Item item in _player.CurrentRoom.ItemsInRoom)
                {
                    Console.WriteLine($"- {item.Name}");
                }
            }
        }
    }

    private void MovePlayer(Room? newRoom)
    {
        if (newRoom != null)
        {
            // Move Player
            _player.CurrentRoom = newRoom;
            _player.VisitedRooms.Add(newRoom);
            DescribeRoom();

            // Trigger combat if it is a monster room
            while (_player.CurrentRoom.MonstersInRoom.Count > 0 && _player.Health > 0)
            {
                Monster monster = _player.CurrentRoom.MonstersInRoom[0];
                bool playerWon = StartCombat(monster);
                if (playerWon)
                {
                    _player.CurrentRoom.MonstersInRoom.Remove(monster);
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            Console.WriteLine("You can't go that way.");
        }
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
                    line += "[#]"; // Visited room
                }
                else
                {
                    line += "   "; // Empty, undiscovered space
                }
            }
            Console.WriteLine(line);
        }
        Console.WriteLine("-------------------");
    }
}