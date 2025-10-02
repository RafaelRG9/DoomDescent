// --- ITEMS AND RECIPES ---

// Crafting materials
Item goblinHide = new Item("Goblin Hide", "Yuck! What did you do to loot this? leathery and tough but, extremely smelly!");

// Craftable Items
Item leatherArmor = new Item("Leather Armor", "Simple armor made from goblin hide, you couldn't get the smell off, nasty!");

// Recipe Definitions, creates a List to hold all receipes in the game
List<CraftingRecipe> availableRecipes = new List<CraftingRecipe>();

// Define the recipe for Leather Armor
CraftingRecipe armorRecipe = new CraftingRecipe(leatherArmor, new Dictionary<Item, int>
{
    {goblinHide, 2} // Requires 2 Goblin hides
});
availableRecipes.Add(armorRecipe);

// --- WORLD AND PLAYER CREATION ---
DungeonGenerator generator = new DungeonGenerator();
Room startingRoom = generator.Generate(10); // Generate a 10-room dungeon
Player player = new Player("Hero", 50, 10, 5); //Name, Health, Strength, Dexterity
player.CurrentRoom = startingRoom; // Place player in origin room
DescribeRoom(player);
// ---------------------------------------------------------------------------------------------------------------------------------------

// --- EXPLORATION GAME LOOP ---
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
            switch (verb)
            {
                case "north":
                    MovePlayer(player.CurrentRoom.North);
                    break;
                case "south":
                    MovePlayer(player.CurrentRoom.South);
                    break;
                case "east":
                    MovePlayer(player.CurrentRoom.East);
                    break;
                case "west":
                    MovePlayer(player.CurrentRoom.West);
                    break;
            }
            break;

        case "look":
        case "l":
            DescribeRoom(player);
            break;
        
        case "stats":
        case "char":
            Console.WriteLine("\n--- Character Stats ---");
            Console.WriteLine($" Name: {player.Name}");
            Console.WriteLine($" Level: {player.Level}");
            Console.WriteLine($" Experience: {player.Experience} / {player.ExperienceToNextLevel}");
            Console.WriteLine($" Health: {player.Health} / {player.MaxHealth}");
            Console.WriteLine("-----------------------");
            Console.WriteLine(" Attributes:");
            foreach (var stat in player.Stats)
            {
                Console.WriteLine($" - {stat.Key}: {stat.Value}");
            }
            Console.WriteLine("-----------------------");
            break;

        case "inventory":
        case "i":
            // display player inventory
            if (player.Inventory.Any())
            {
                Console.WriteLine("\nInventory");
                foreach (var item in player.Inventory)
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
                Item? itemToExamine = player.Inventory.Find(item => item.Name.Equals(itemToExamineName, StringComparison.OrdinalIgnoreCase));

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
            if (words.Length > 1)
            {
                string itemToTakeName = string.Join(" ", words.Skip(1));
                Item? itemToTake = player.CurrentRoom.ItemsInRoom.Find(item => item.Name.Equals(itemToTakeName, StringComparison.OrdinalIgnoreCase));

                if (itemToTake != null)
                {
                    player.Inventory.Add(itemToTake);
                    Console.WriteLine($"You took the {itemToTake.Name}");
                    player.CurrentRoom.ItemsInRoom.Remove(itemToTake);
                }
            }
            break;

            case "drop":
            if (words.Length > 1)
            {
                string itemToDropName = string.Join(" ", words.Skip(1));
                Item? itemToDrop = player.Inventory.Find(item => item.Name.Equals(itemToDropName, StringComparison.OrdinalIgnoreCase));

                if (itemToDrop != null)
                {
                    player.Inventory.Remove(itemToDrop);
                    Console.WriteLine($"You dropped the {itemToDrop.Name}");
                    player.CurrentRoom.ItemsInRoom.Add(itemToDrop);
                }
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



// --- HELPER METHODS ---

static bool StartCombat(Player player, Monster monsterToFight) // Will be used in Main loop once combat is refactored as a random event in rooms
{
    Console.WriteLine($"{player.Name} encounters a fierce {monsterToFight.Name}");

    // Turn-Based combat
    while (player.Health > 0 && monsterToFight.Health > 0)
    {
        // Display Status
        Console.WriteLine("\n--------------------");
        Console.WriteLine($"{player.Name}: {player.Health}/{player.MaxHealth}");
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
            // Calculate Damage
            int damageDealt = player.Stats["Strength"];
            monsterToFight.Health -= damageDealt;
            Console.WriteLine($"You attack the {monsterToFight.Name}, dealing {damageDealt} damage!");
        }
        else
        {
            Console.WriteLine("Invalid command! You hesitate and lose your turn.");
        }

        // Check if Monster still lives and calculate their attack
        if (monsterToFight.Health > 0)
        {
            int damageTaken = monsterToFight.Stats["Strength"];
            player.Health -= damageTaken;
            Console.WriteLine($"The {monsterToFight.Name} retaliates, dealing {damageTaken} damage to you!");
        }
    }

    Console.WriteLine("\n--- BATTLE OVER ---");

    // Check for winners
    if (player.Health > 0)
    {
        Console.WriteLine($"You defeated the {monsterToFight.Name}!");
        if (monsterToFight.Loot != null)
        {
            Console.WriteLine($"You found a {monsterToFight.Loot.Name}!");
            player.Inventory.Add(monsterToFight.Loot);
        }
        player.AddExperience(monsterToFight.ExperienceValue);

        return true;
    }
    else
    {
        return false;
    }
}

void DescribeRoom(Player player)
{
    if (player.CurrentRoom == null)
    {
        Console.WriteLine("You are lost in a void...");
        return;
    }

    Console.WriteLine($"\nLocation: {player.CurrentRoom.Name} ({player.CurrentRoom.X}, {player.CurrentRoom.Y})");
    Console.WriteLine(player.CurrentRoom.Description);

    // --- MONSTER OR ITEM LIST ---
    if (player.CurrentRoom.MonstersInRoom.Count > 0)
    {
        Console.WriteLine("\nDangerous creatures lurk here:");
        foreach (Monster monster in player.CurrentRoom.MonstersInRoom)
        {
            Console.WriteLine($"- A fearsome {monster.Name}");
        }
    }
    else
    {
        if (player.CurrentRoom.ItemsInRoom.Count > 0)
        {
            Console.WriteLine("\nYou found treasure in this room:");
            foreach (Item item in player.CurrentRoom.ItemsInRoom)
            {
                Console.WriteLine($"- {item.Name}");
            }
        }
    }
}

void MovePlayer(Room? newRoom)
{
    if (newRoom != null)
    {
        // Move Player
        player.CurrentRoom = newRoom;
        DescribeRoom(player);

        // Trigger combat if it is a monster room
        while (player.CurrentRoom.MonstersInRoom.Count > 0 && player.Health > 0)
        {
            Monster monster = player.CurrentRoom.MonstersInRoom[0];
            bool playerWon = StartCombat(player, monster);
            if (playerWon)
            {
                player.CurrentRoom.MonstersInRoom.Remove(monster);
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