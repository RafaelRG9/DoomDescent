using csharp_roguelike_rpg.Characters;
using csharp_roguelike_rpg.Systems;
namespace csharp_roguelike_rpg.Abilities;

public class DoubleStrike : Ability
{
    public DoubleStrike()
    {
        Name = "Double Strike";
        Description = "You sly fox quick as a fox (redundant as hitting twice). Hit the target twice for 75% of your Dexterity as damage";
        EnergyCost = 5;
    }

    public override void Use(Character caster, Character target)
    {
        // Each hit does 75% of the caster's Dexterity as damage
        int damagePerHit = (int)(caster.Stats["Dexterity"] * 0.75);

        // First Hit
        target.Health -= damagePerHit;
        UIManager.SlowPrint($"{caster.Name} strikes quickly, dealing {damagePerHit} damage!", ConsoleColor.Yellow);

        // If target is still alive, hit a second time
        if (target.Health > 0)
        {
            target.Health -= damagePerHit;
            UIManager.SlowPrint($"{caster.Name} follows up with a second strike, dealing {damagePerHit} damage!", ConsoleColor.Yellow);
        }
    }
}