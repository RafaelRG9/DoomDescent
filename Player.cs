public class Player : Character
{
    public int Level { get; set; }
    public int Experience { get; set; }
    public int ExperienceToNextLevel { get; set; }
    public List<Item> Inventory { get; private set; }

    public Player(string name, int health, int strength, int dexterity)
        : base(name, health, strength, dexterity)
    {
        Level = 1;
        Experience = 0;
        ExperienceToNextLevel = 100;
        Inventory = new List<Item>();
    }

    public void AddExperience(int experienceGained)
    {
        Experience += experienceGained;
        Console.WriteLine($"You gained {experienceGained} experience!");

        // Check if player has enough XP to level Up
        // Using a loop to carryover overflowing XP
        while (Experience >= ExperienceToNextLevel)
        {
            // Level up!
            Level++;
            Console.WriteLine($"\n*** You have reached Level {Level}! ***");

            // Keep any remainder XP
            Experience -= ExperienceToNextLevel;

            // Increase stats, static for now bu formulaic later
            int healthIncrease = 10;
            int strengthIncrease = 2;
            int dexterityIncrease = 1;

            MaxHealth += healthIncrease;
            Stats["Strength"] += strengthIncrease;
            Stats["Dexterity"] += dexterityIncrease;

            // Fully heal player
            Health = MaxHealth;

            Console.WriteLine($"Max Health increased by {healthIncrease}.");
            Console.WriteLine($"Strength increased by {strengthIncrease}.");
            Console.WriteLine($"Dexterity increased by {dexterityIncrease}.");

            // Calculate XP for next level
            ExperienceToNextLevel = Level * 100;
            Console.WriteLine($"Experience to next level: {ExperienceToNextLevel}");
        }
    }
}