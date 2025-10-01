// Creating a functioning combat arena

// Just one item for the onster to drop
Item rustySword = new Item("Rusty Sword", "A sword that has seen better days");

// Combatants
Player player = new Player("Hero", 50, 10, 5); //Name, Health, Strength, Dexterity
Monster goblin = new Monster("Goblin", 20, 8, 3, rustySword); // Name, Health, Strength, Dexterity, Loot

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
    string playerChoice = Console.ReadLine();

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
    }
}
else
{
    Console.WriteLine("You have been defeated...");
}