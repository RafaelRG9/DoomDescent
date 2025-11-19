using csharp_roguelike_rpg.Characters;
using csharp_roguelike_rpg.Systems;

namespace csharp_roguelike_rpg.Abilities;

public class BodySlam : Ability
{
    public BodySlam()
    {
        Name = "Body Slam";
        Description = "Throw your weight around. Deals damage equal to 33% of your Max Health.";
        EnergyCost = 8;
    }

    public override void Use(Character caster, Character target)
    {
        // 33% of Max Health
        int damage = caster.MaxHealth / 3;

        target.Health -= damage;

        UIManager.SlowPrint($"{caster.Name} throws their entire body weight at {target.Name}!", ConsoleColor.DarkYellow);
        UIManager.SlowPrint($"CRUNCH! Deals {damage} damage.", ConsoleColor.Yellow);
    }
}