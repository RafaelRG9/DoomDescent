using csharp_roguelike_rpg.Characters;
using csharp_roguelike_rpg.Systems;

namespace csharp_roguelike_rpg.Abilities;

public class ChaosBolt : Ability
{
    public ChaosBolt()
    {
        Name = "Chaos Bolt";
        Description = "A shimmering bolt of unstable energy. Deals 1 to 500% Intellect damage.";
        EnergyCost = 5;
    }

    public override void Use(Character caster, Character target)
    {
        Random rng = new Random();
        
        int minDamage = 1;
        int maxDamage = caster.Stats["Intellect"] * 5;

        int damage = rng.Next(minDamage, maxDamage + 1);

        target.Health -= damage;
        
        
        if (damage < caster.Stats["Intellect"])
        {
            UIManager.SlowPrint($"{caster.Name} fires a Chaos Bolt... but it fizzles! Only {damage} damage.", ConsoleColor.Gray);
        }
        else if (damage > caster.Stats["Intellect"] * 4)
        {
            UIManager.SlowPrint($"{caster.Name} fires a Chaos Bolt... CRITICAL SURGE! {damage} damage!", ConsoleColor.Magenta);
        }
        else
        {
            UIManager.SlowPrint($"{caster.Name} fires a Chaos Bolt dealing {damage} damage.", ConsoleColor.Blue);
        }
    }
}