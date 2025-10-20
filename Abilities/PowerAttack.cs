using csharp_roguelike_rpg.Characters;
using csharp_roguelike_rpg.Systems;
namespace csharp_roguelike_rpg.Abilities;

public class PowerAttack : Ability
{
    public PowerAttack()
    {
        Name = "Power Attack";
        Description = "Hurr Durr Warrior hit strong. Deals 150% of Strength as damage";
        EnergyCost = 5;
    }

    public override void Use(Character caster, Character target)
    {
        // Power Attack deals 150% of the caster's Strength as damage
        int damage = (int)(caster.GetTotalStrength() * 1.5);
        target.Health -= damage;
        UIManager.SlowPrint($"{caster.Name} uses {Name}, dealing {damage} damage to {target.Name}!");
    }
}