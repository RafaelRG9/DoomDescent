using csharp_roguelike_rpg.Abilities;
using csharp_roguelike_rpg.Items;
using csharp_roguelike_rpg.Systems;
using csharp_roguelike_rpg.World;
namespace csharp_roguelike_rpg.Characters;

public class Player : Character
{
    public PlayerClass Class { get; private set; }
    public int Level { get; set; }
    public int Experience { get; set; }
    public int ExperienceToNextLevel { get; set; }
    public Room? CurrentRoom { get; set; }
    public List<Item> Inventory { get; private set; }
    public Dictionary<EquipmentSlot, Item> Equipment { get; private set; }
    public HashSet<Room> VisitedRooms { get; private set; }
    public List<Ability> Abilities { get; private set; }

    public Player(string name, PlayerClass playerClass)
        : base(name, 0, 0, 0, 0)
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
                Stats["Intellect"] = 3;
                Abilities.Add(new PowerAttack());
                break;
            case PlayerClass.Rogue:
                MaxHealth = 60;
                Stats["Strength"] = 8;
                Stats["Dexterity"] = 10;
                Stats["Intellect"] = 5;
                Abilities.Add(new DoubleStrike());
                break;
            case PlayerClass.Mage:
                MaxHealth = 50;
                Stats["Strength"] = 5;
                Stats["Dexterity"] = 6;
                Stats["Intellect"] = 12;
                Abilities.Add(new Fireball());
                break;
        }
        Health = MaxHealth;
        Class = playerClass;
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

    public bool AddExperience(int experienceGained)
    {
        Experience += experienceGained;
        bool leveledUp = false;

        while (Experience >= ExperienceToNextLevel)
        {
            // Level up!
            leveledUp = true;
            Level++;
            Experience -= ExperienceToNextLevel;

            // --- Class-based stat increases ---
            int healthIncrease = 0;
            int strengthIncrease = 0;
            int dexterityIncrease = 0;
            int intellectIncrease = 0;

            switch (Class)
            {
                case PlayerClass.Warrior:
                    healthIncrease = 15;
                    strengthIncrease = 3;
                    dexterityIncrease = 1;
                    intellectIncrease = 0;
                    break;
                case PlayerClass.Rogue:
                    healthIncrease = 10;
                    strengthIncrease = 1;
                    dexterityIncrease = 3;
                    intellectIncrease = 1;
                    break;
                case PlayerClass.Mage:
                    healthIncrease = 8;
                    strengthIncrease = 1;
                    dexterityIncrease = 1;
                    intellectIncrease = 3;
                    break;
            }

            MaxHealth += healthIncrease;
            Stats["Strength"] += strengthIncrease;
            Stats["Dexterity"] += dexterityIncrease;
            Stats["Intellect"] += intellectIncrease;
            Health = MaxHealth;
            ExperienceToNextLevel = Level * 100;
        }
        return leveledUp;
    }
}