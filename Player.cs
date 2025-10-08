public class Player : Character
{
    public int Level { get; set; }
    public int Experience { get; set; }
    public int ExperienceToNextLevel { get; set; }
    public Room? CurrentRoom { get; set; }
    public List<Item> Inventory { get; private set; }
    public Dictionary<EquipmentSlot, Item> Equipment { get; private set; }
    public HashSet<Room> VisitedRooms { get; private set; }
    public List<Ability> Abilities { get; private set; }

    public Player(string name, PlayerClass playerClass)
        : base(name, 0, 0, 0)
    {
        Level = 1;
        Experience = 0;
        ExperienceToNextLevel = 100;
        Inventory = new List<Item>();
        Equipment = new Dictionary<EquipmentSlot, Item>();
        VisitedRooms = new HashSet<Room>();
        Abilities = new List<Ability>();

        switch (playerClass)
        {
            case PlayerClass.Warrior:
                MaxHealth = 80;
                Stats["Strength"] = 12;
                Stats["Dexterity"] = 4;
                Abilities.Add(new PowerAttack());
                break;
            case PlayerClass.Rogue:
                MaxHealth = 60;
                Stats["Strength"] = 8;
                Stats["Dexterity"] = 10;
                break;
            case PlayerClass.Mage:
                MaxHealth = 50;
                Stats["Strength"] = 5;
                Stats["Dexterity"] = 6;
                // TODO: Add an "Intellect" stat for mages
                break;
        }
        Health = MaxHealth;
    }

    public int GetTotalStrength()
    {
        int totalStrength = Stats["Strength"];

        // Check for an equipped weapon
        if (Equipment.TryGetValue(EquipmentSlot.MainHand, out Item? equippedItem))
        {
            if (equippedItem is Weapon equippedWeapon)
            {
                // Add weapon's damage
                totalStrength += equippedWeapon.Damage;
            }
        }
        return totalStrength;
    }

    public int GetTotalDefense()
    {
        int totalDefense = 0;

        // Check for an equipped armor
        if (Equipment.TryGetValue(EquipmentSlot.Chest, out Item? equippedItem))
        {
            if (equippedItem is Armor equippedArmor)
            {
                // Add Armor's defense
                totalDefense += equippedArmor.Defense;
            }
        }
        return totalDefense;
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