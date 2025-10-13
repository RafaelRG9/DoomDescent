using csharp_roguelike_rpg.Characters;
using csharp_roguelike_rpg.Items;
using csharp_roguelike_rpg.Systems;

public class GameData
{
    public Item? StairsDown { get; private set; }
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
        Item orcTusk = new Item("Orc Tusk", "surprisingly clean, this Orc flossed!");
        Item boneChips = new Item("Bone Chips", "Probably not useful for soup.");
        Item corrosiveOoze = new Item("Corrosive Ooze", "A vial of bubbling green goo. Don't drink it. Seriously.");
        Item impHeart = new Item("Imp's Heart", "Still warm and faintly glowing. Probably useful in some forbidden recipe, or trash, your call.");
        Item spiderSilk = new Item("Spider Silk", "Strong and sticky. Maybe could be woven into some light armor.");

        // Crafted Items
        Armor leatherArmor = new Armor("Leather Armor", "Simple armor made from goblin hide, you couldn't get the smell off, nasty!. Gves +2 Defense", 2);
        Armor silkArmor = new Armor("Silk Armor", "How did you even weave this yourself? I guess 'cause magic!. Gives +3 Defense", 3);
        Weapon boneSword = new Weapon("Bone Sword", "Looks flimsy, better than nothing, I guess. +2 Damage.", 2);
        Weapon orcTuskSword = new Weapon("Orc Tusk Sword", "Now we are talking, something a little more damaging. +6 Damage.", 6);

        // Weapon drops
        Weapon ogreClub = new Weapon("Ogre's Club", "The only thing they know how to use well. +5 Damage.", 5);
        Weapon dragonClaw = new Weapon("Dragon Claw Dagger", "Nothing says baddass like a DRAGON CLAW for a weapon. Deals 15 damage.", 15);

        // --- SPECIAL ITEMS ---
        Item StairsDown = new Item("Stairs Down", "A staircase leading deeper into the darkness...");


        //----------------------------------------------------------------------------------------------------------------------------------------------
        // --- TABLE CONTENTS ---
        //----------------------------------------------------------------------------------------------------------------------------------------------        

        // --- MASTER LOOT TABLE ---
        MasterLootTable.Add(new Armor("Rusty Armor", "Well, better than nothing!", 2));
        MasterLootTable.Add(new Weapon("Rusty Sword", "An old sword, as sharp as rolling pin!. At least it could give Tetanus", 2));
        MasterLootTable.Add(new Potion("Small Potion", "Wouldn't hurt to drink this, wouldn't help either (Restores 20 health, impressive...)", 20));
        MasterLootTable.Add(goblinHide);
        MasterLootTable.Add(orcTusk);


        // --- MASTER MONSTER TABLE ---
        MasterMonsterTable.Add(new Goblin(goblinHide));
        MasterMonsterTable.Add(new Orc(orcTusk));
        MasterMonsterTable.Add(new GiantSpider(spiderSilk));
        MasterMonsterTable.Add(new AnimatedSkeleton(boneChips));
        MasterMonsterTable.Add(new CorrosiveSlime(corrosiveOoze));
        MasterMonsterTable.Add(new FireImp(impHeart));
        MasterMonsterTable.Add(new Ogre(ogreClub));


        // --- MASTER BOSS TABLE ---
        MasterBossTable.Add(new Boss(dragonClaw));

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
            new StatModifier("Thick Hide", "Who needs brains when you have brawn? and you have neither. Permanently increases Max Health by 15.", Rarity.Uncommon, "MaxHealth", 15, "null",  0),
            new StatModifier("Weapon Master", "You smash things harder now, woop!. Permanently increases Strength by 2.", Rarity.Uncommon, "Strength", 2, "null", 0),
            new StatModifier("Adrenaline Rush", "You feel less pain now, big deal. Permanently increases Dexterity by 1.", Rarity.Common, "Dexterity", 1, "null", 0)
        },
            // Level 6 Talents
            [6] = new List<Modifier>
        {
            new StatModifier("Juggernaut", "Moar brawn, but this time, less brains. Permanently increases Max Health by 20 and reduces Intellect by 5.", Rarity.Uncommon, "MaxHealth", 20, "Intellect", 5),
            new StatModifier("Berserker", "Who needs full health when you can go faster?. Permanently increases Dexterity by 2.", Rarity.Uncommon, "Dexterity", 2, "null", 0),
            new StatModifier("Mighty Blow", "ME BIG STRONG, HIT THINGS HARDER!. Permanently increases Strength by 3.", Rarity.Epic, "Strength", 3, "null", 0)
        },
            // Level 9 Talents
            [9] = new List<Modifier>
        {
            new StatModifier("Unstoppable", "You just won't die, won't you?. Permanently increases Max Health by 25.", Rarity.Epic, "MaxHealth", 25, "null", 0),
            new StatModifier("Whirlwind", "Fan goes hurr durr. Permanently increases Dexterity by 3.", Rarity.Epic, "Dexterity", 3, "null", 0),
            new StatModifier("Colossal Strength", "ME DA STRONGEST, NOT THE INTELLIGENTEST. Permanently increases Strength by 10, Intellect reduced by 2.", Rarity.Legendary, "Strength", 4, "Intellect", 2)
        }
        };

        // --- ROGUE TALENTS ---
        ClassTalents[PlayerClass.Rogue] = new Dictionary<int, List<Modifier>>
        {
            // Level 3 Talents
            [3] = new List<Modifier>
        {
            new StatModifier("Fleet Footed", "I AM SPEED!. Permanently increases Dexterity by 2.", Rarity.Uncommon, "Dexterity", 2, "null", 0),
            new StatModifier("Precise Strikes", "Your stab got stabbier!. Permanently increases Strength by 1.", Rarity.Common, "Strength", 1, "null", 0),
            new StatModifier("Toughness", "You are not as feeble as I thought. Permanently increases Max Health by 10.", Rarity.Common, "MaxHealth", 10, "null", 0)
        },
            // Level 6 Talents
            [6] = new List<Modifier>
            {
            new StatModifier("Evasion", "You can't C me!. Permanently increases Dexterity by 3.", Rarity.Uncommon, "Dexterity", 3, "null", 0),
            new StatModifier("Killer Instinct", "Kill all the things!. Permanently increases Strength by 2.", Rarity.Uncommon, "Strength", 2, "null", 0),
            new StatModifier("Iron Will", "Really, you are going for a tanky build? whatever!. Permanently increases Max Health by 15.", Rarity.Common, "MaxHealth", 15, "null", 0)
            },
            // Level 9 Talents
            [9] = new List<Modifier>
            {
            new StatModifier("Shadowstep", "Sneaky stabby!. Permanently increases Dexterity by 4.", Rarity.Epic, "Dexterity", 4, "null", 0),
            new StatModifier("Assassinate", "Kill kill kill. Permanently increases Strength by 3.", Rarity.Epic, "Strength", 3, "null", 0),
            new StatModifier("Stone Skin", "Why do you even pick me?. Permanently increases Max Health by 20.", Rarity.Epic, "MaxHealth", 20, "null", 0)
            }
        };

        // --- MAGE TALENTS ---
        ClassTalents[PlayerClass.Mage] = new Dictionary<int, List<Modifier>>
        {
            // Level 3 Talents for Mage
            [3] = new List<Modifier>
        {
            new StatModifier("Arcane Intellect", "You are smarter than a fifth grader!. Permanently increases Intellect by 3.", Rarity.Uncommon, "Intellect", 3, "null", 0),
            new StatModifier("Glass Cannon", "Do I really need to explain this?. +5 Intellect, -5 Max Health.", Rarity.Epic, "Intellect", 5, "MaxHealth", 5),
            new StatModifier("Stamina", "Basic white mage buff. Permanently increases Max Health by 12.", Rarity.Common, "MaxHealth", 12, "null", 0)
        },
            // Level 6 Talents for Mage
            [6] = new List<Modifier>
        {
            new StatModifier("Elemental Mastery", "You are even smarter than a fifth grader!. Permanently increases Intellect by 5.", Rarity.Epic, "Intellect", 5, "null", 0),
            new StatModifier("Mana Fountain", "You can cast more fireballs, how original!. Permanently increases Max Energy by 20.", Rarity.Uncommon, "MaxEnergy", 20, "null", 0),
            new StatModifier("Quick Thinking", "You are quick as a magical fox. Permanently increases Dexterity by 2.", Rarity.Uncommon, "Dexterity", 2, "null", 0)
        },
            // Level 9 Talents for Mage
            [9] = new List<Modifier>
        {
            new StatModifier("Archmage", "You are the smartest fifth grader ever!. Permanently increases Intellect by 5.", Rarity.Legendary, "Intellect", 5, "null", 0),
            new StatModifier("Manaforged", "Your magical reserves are unlimited!, not really. Permanently increases Max Energy by 25.", Rarity.Epic, "MaxEnergy", 25, "null", 0),
            new StatModifier("Glassier Cannon", "all brains and no brawn. Permanently increases Intellect by 10, decreases health by 10.", Rarity.Epic, "Intellect", 10, "MaxHealth", 10)
        }
        };
    }
}