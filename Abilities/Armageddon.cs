using csharp_roguelike_rpg.Characters;
using csharp_roguelike_rpg.Systems;

namespace csharp_roguelike_rpg.Abilities;

public class Armageddon : Ability
{
    public Armageddon()
    {
        Name = "Armageddon";
        Description = "Consumes ALL current Energy. Deals 5 damage for every point of Energy consumed.";
        EnergyCost = 0; // drains all mana, handled bellow
    }

    public override void Use(Character caster, Character target)
    {
        int energyConsumed = caster.Energy;
        int damage = energyConsumed * 5;

        caster.Energy = 0;
        target.Health -= damage;

        UIManager.SlowPrint($"{caster.Name} channels every ounce of power into a cataclysmic blast!", ConsoleColor.DarkRed);
        UIManager.SlowPrint($"ARMAGEDDON deals {damage} damage!", ConsoleColor.Red);
    }
}