using csharp_roguelike_rpg.Characters;

namespace csharp_roguelike_rpg.Systems;

public static class UIManager
{
    public static void SlowPrint(string text, ConsoleColor? color = null)
    {
        if (color.HasValue)
        {
            Console.ForegroundColor = color.Value;
        }

        foreach (char c in text)
        {
            Console.Write(c);
            Thread.Sleep(25);
        }

        Console.ResetColor();
        Console.WriteLine();
    }

    public static void DisplayStats(Player player)
    {
        Console.WriteLine("\n--- Character Stats ---");
        Console.WriteLine($" Name: {player.Name}");
        Console.WriteLine($" Class: {player.Class}");
        Console.WriteLine($" Level: {player.Level}");
        Console.WriteLine($" Experience: {player.Experience} / {player.ExperienceToNextLevel}");
        Console.WriteLine($" Health: {player.Health} / {player.MaxHealth}");
        Console.WriteLine($" Energy: {player.Energy} / {player.MaxEnergy}");
        Console.WriteLine("-----------------------");
        Console.WriteLine(" Attributes:");
        foreach (var stat in player.Stats)
        {
            Console.WriteLine($" - {stat.Key}: {stat.Value}");
        }
        Console.WriteLine(" Abilities:");
        foreach (var ability in player.Abilities)
        {
            Console.WriteLine($" - {ability.Name}(Cost: {ability.EnergyCost}): {ability.Description}");
        }
        Console.WriteLine("-----------------------");
    }

    public static void DisplayInventory(Player player)
    {
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
}