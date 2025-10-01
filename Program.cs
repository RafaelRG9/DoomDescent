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

// --- PLAYER CREATION ---
Player player = new Player("Hero", 50, 10, 5); //Name, Health, Strength, Dexterity
// ---------------------------------------------------------------------------------------------------------------------------------------


// --- MAIN GAME LOOP ---
while (true)
{
    Console.WriteLine("\nYou are in the safe camp. What do you do?");
    Console.WriteLine("- explore");
    Console.WriteLine("- craft");
    Console.WriteLine("- stats/char");
    Console.WriteLine("- examine/x (check an item in your inventory)");
    Console.WriteLine("- quit");
    Console.Write("> ");
    string? choice = Console.ReadLine();

    // String safety check, making sure choice does not have a null vaue passed to it
    if (string.IsNullOrEmpty(choice))
    {
        Console.WriteLine("Please enter a command.");
        continue; // Restart the loop
    }

    if (choice == "explore")
    {
        // When the player explores, a battle begins
        bool playerWon = StartCombat(player, goblinHide);
        if (!playerWon)
        {
            Console.WriteLine("Game Over");
            break; // Exit the main game loop if the player dies.
        }
    }
    else if (choice == "craft")
    {
        // --- CRAFTING LOOP ---
        while (true)
        {
            Console.WriteLine("\n--- SAFE AREA ---");
            Console.WriteLine("you can now craft. Options:");
            Console.WriteLine("- craft leather armor");
            Console.WriteLine("- i (view inventory)");
            Console.WriteLine("- leave");
            Console.Write("> ");
            string? craftingChoice = Console.ReadLine();

            if (string.IsNullOrEmpty(craftingChoice)) continue;

            if (craftingChoice.Equals("leave", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("You leave the area, ready for the next adventure");
                break; // Exists crafting loop
            }
            else if (craftingChoice.Equals("i", StringComparison.OrdinalIgnoreCase) || craftingChoice.Equals("inventory", StringComparison.OrdinalIgnoreCase))
            {
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
            }
            else if (craftingChoice.StartsWith("craft ", StringComparison.OrdinalIgnoreCase))
            {
                // --- CRAFTING LOGIC ---
                string itemToCraftName = craftingChoice.Substring(6);

                // Find the recipe
                CraftingRecipe? recipe = availableRecipes.Find(r => r.ResultingItem.Name.Equals(itemToCraftName, StringComparison.OrdinalIgnoreCase));

                if (recipe == null)
                {
                    Console.WriteLine("You don't know how to craft that.");
                    continue;
                }
                // Check if player has the required materials
                bool canCraft = true;
                foreach (var material in recipe.RequiredMaterials)
                {
                    int materialCount = player.Inventory.Count(item => item.Name == material.Key.Name);
                    if (materialCount < material.Value)
                    {
                        Console.WriteLine($"You don't have enough {material.Key.Name}. Required: {material.Value}, Have: {materialCount}");
                        canCraft = false;
                        break;
                    }
                }
                // If enough materials, craft the item
                if (canCraft)
                {
                    // Consume materials
                    foreach (var material in recipe.RequiredMaterials)
                    {
                        for (int i = 0; i < material.Value; i++)
                        {
                            Item itemToRemove = player.Inventory.First(item => item.Name == material.Key.Name);
                            player.Inventory.Remove(itemToRemove);
                        }
                    }
                    // Add new item to inventory
                    player.Inventory.Add(recipe.ResultingItem);
                    Console.WriteLine($"Successfully crafted {recipe.ResultingItem.Name}!");
                }
            }
        }
    }
    else if (choice == "stats" || choice == "char")
    {
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
    }
    else if (choice.StartsWith("examine ", StringComparison.OrdinalIgnoreCase) || choice.StartsWith("x ", StringComparison.OrdinalIgnoreCase))
    {
        string[] words = choice.Split(' ');
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
    }
    else if (choice == "quit")
    {
        Console.WriteLine("You decide to rest for now. Until next time!");
        break;
    }
}

// --- HELPER METHODS ---

static bool StartCombat(Player player, Item LootItem)
{
    // Combatants
    Monster goblin = new Monster("Goblin", 20, 8, 3, 50, LootItem); // Name, Health, Strength, Dexterity, XP, Loot

    Console.WriteLine($"{player.Name} encounters a fierce {goblin.Name}");

    // Turn-Based combat
    while (player.Health > 0 && goblin.Health > 0)
    {
        // Display Status
        Console.WriteLine("\n--------------------");
        Console.WriteLine($"{player.Name}: {player.Health}/{player.MaxHealth}");
        Console.WriteLine($"{goblin.Name}: {goblin.Health}/{goblin.MaxHealth}");
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
            goblin.Health -= damageDealt;
            Console.WriteLine($"You attack the {goblin.Name}, dealing {damageDealt} damage!");
        }
        else
        {
            Console.WriteLine("Invalid command! You hesitate and lose your turn.");
        }

        // Check if Monster still lives and calculate their attack
        if (goblin.Health > 0)
        {
            int damageTaken = goblin.Stats["Strength"];
            player.Health -= damageTaken;
            Console.WriteLine($"The {goblin.Name} retaliates, dealing {damageTaken} damage to you!");
        }
    }

    Console.WriteLine("\n--- BATTLE OVER ---");

    // Check for winners
    if (player.Health > 0)
    {
        Console.WriteLine($"You defeated the {goblin.Name}!");
        if (goblin.Loot != null)
        {
            Console.WriteLine($"You found a {goblin.Loot.Name}!");
            player.Inventory.Add(goblin.Loot);
        }
        player.AddExperience(goblin.ExperienceValue);

        return true;
    }
    else
    {
        return false;
    }
}
