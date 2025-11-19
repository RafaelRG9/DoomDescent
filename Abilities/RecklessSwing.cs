using csharp_roguelike_rpg.Characters;
using csharp_roguelike_rpg.Systems;

namespace csharp_roguelike_rpg.Abilities;

public class RecklessSwing : Ability
{
    public RecklessSwing()
    {
        Name = "Reckless Swing";
        Description = "Who cares about safety when you can pull BIG NUMBERS?. Deal 250% Strangth damage, but take 5 damage yourself";
        EnergyCost = 6;
    }

    public override void Use(Character caster, Character target)
    {
        int damage = (int)(caster.GetTotalStrength() * 2.5);
        int selfDamage = 5;

        target.Health -= damage;
        caster.Health -= selfDamage;

        UIManager.SlowPrint($"{caster.Name} swings wildly! Dealing {damage} damage to the enemy...", ConsoleColor.Red);
        UIManager.SlowPrint($"...but hurts themselves for {selfDamage} in the process!", ConsoleColor.DarkRed);
    }
}