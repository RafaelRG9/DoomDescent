using csharp_roguelike_rpg.Characters;
using csharp_roguelike_rpg.Systems;

namespace csharp_roguelike_rpg.Abilities;

public class Execute : Ability
{
    public Execute()
    {
        Name = "Execute";
        Description = "Mercy is for the weak!. Deals massive damage to low health targets. 4x damage if target is below 30% Health";
        EnergyCost = 10;
    }

    public override void Use(Character caster, Character target)
    {
        int damage = caster.GetTotalStrength();

        // Health threshold (30%)
        int executeThreshold = (int)(target.MaxHealth * 0.3);

        if (target.Health < executeThreshold)
        {
            damage *= 4;
            UIManager.SlowPrint($"{caster.Name} sees a weakness and EXECUTES!", ConsoleColor.Red);
        }
        else
        {
            UIManager.SlowPrint($"{caster.Name} tries to execute, but the enemy is too healthy.", ConsoleColor.Gray);
        }

        target.Health -= damage;
        UIManager.SlowPrint($"The blow deals {damage} damage!");
    }
}