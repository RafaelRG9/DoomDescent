public class GameData
{
    public List<Item> MasterLootTable { get; private set; }
    public List<Monster> MasterMonsterTable { get; private set; }
    public List<Monster> MasterBossTable { get; private set; }
    public List<CraftingRecipe> AvailableRecipes { get; private set; }
    public Dictionary<PlayerClass, Dictionary<int, List<Modifier>>> ClassTalents { get; private set; }

    public GameData()
    {
        // --- INITIALIZE TABLES ---
        MasterLootTable = new List<Item>();
        MasterMonsterTable = new List<Monster>();
        MasterBossTable = new List<Monster>();
        AvailableRecipes = new List<CraftingRecipe>();
        ClassTalents = new Dictionary<PlayerClass, Dictionary<int, List<Modifier>>>();

        // --- CRAFTED ITEMS AND MATERIALS ---

        // Materials
        Item goblinHide = new Item("Goblin Hide", "Yuck! What did you do to loot this? leathery and tough but, extremely smelly!");

        // Crafted Items
        Item leatherArmor = new Item("Leather Armor", "Simple armor made from goblin hide, you couldn't get the smell off, nasty!");


        //----------------------------------------------------------------------------------------------------------------------------------------------
        // --- TABLE CONTENTS ---
        //----------------------------------------------------------------------------------------------------------------------------------------------        

        // --- MASTER LOOT TABLE ---
        MasterLootTable.Add(new Armor("Rusty Armor", "Well, better than nothing!", 2));
        MasterLootTable.Add(new Weapon("Rusty Sword", "An old sword, as sharp as rolling pin!. At least it could give Tetanus", 2));
        MasterLootTable.Add(new Potion("Small Potion", "Wouldn't hurt to drink this, wouldn't help either (Restores 20 health, impressive...)", 20));
        MasterLootTable.Add(goblinHide);


        // --- MASTER MONSTER TABLE ---
        MasterMonsterTable.Add(new Goblin(goblinHide));
        MasterMonsterTable.Add(new Orc(null));


        // --- MASTER BOSS TABLE ---
        MasterBossTable.Add(new Boss(null));

        // --- CRAFTING TABLE ---
        CraftingRecipe armorRecipe = new CraftingRecipe(leatherArmor, new Dictionary<Item, int>
        {
            { goblinHide, 2 }
        });
        AvailableRecipes.Add(armorRecipe);

        //----------------------------------------------------------------------------------------------------------------------------------------------
        // --- TALENTS ---
        //----------------------------------------------------------------------------------------------------------------------------------------------

        // --- WARRIOR TALENTS ---
        ClassTalents[PlayerClass.Warrior] = new Dictionary<int, List<Modifier>>
        {
            // Level 3 Talents
            [3] = new List<Modifier>
        {
            new StatModifier("Thick Hide", "Who needs brains when you have brawn? and you have neither. Permanently increases Max Health by 15.", Rarity.Uncommon, "MaxHealth", 15),
            new StatModifier("Weapon Master", "You smash things harder now, woop!. Permanently increases Strength by 2.", Rarity.Uncommon, "Strength", 2),
            new StatModifier("Adrenaline Rush", "You feel less pain now, big deal. Permanently increases Dexterity by 1.", Rarity.Common, "Dexterity", 1)
        }
            // TODO: Add extra tiers
        };

        // --- ROGUE TALENTS ---
        ClassTalents[PlayerClass.Rogue] = new Dictionary<int, List<Modifier>>
        {
            // Level 3 Talents
            [3] = new List<Modifier>
        {
            new StatModifier("Fleet Footed", "I AM SPEED!. Permanently increases Dexterity by 2.", Rarity.Uncommon, "Dexterity", 2),
            new StatModifier("Precise Strikes", "Your stab got stabbier!. Permanently increases Strength by 1.", Rarity.Common, "Strength", 1),
            new StatModifier("Toughness", "You are not as feeble as you think. Permanently increases Max Health by 10.", Rarity.Common, "MaxHealth", 10)
        }
        };

        // --- MAGE TALENTS ---
        ClassTalents[PlayerClass.Mage] = new Dictionary<int, List<Modifier>>
        {
            // Level 3 Talents for Mage
            [3] = new List<Modifier>
        {
            new StatModifier("Arcane Intellect", "You are smarter than a fifth grader!. Permanently increases Intellect by 3.", Rarity.Uncommon, "Intellect", 3),
            new StatModifier("Glass Cannon", "Do I really need to explain this?. +2 Intellect, -5 Max Health.", Rarity.Epic, "Intellect", 2), // TODO: update StatModifier to handle negative values on other stats
            new StatModifier("Stamina", "Basic white mage buff. Permanently increases Max Health by 12.", Rarity.Common, "MaxHealth", 12)
        }
        };
    }
}