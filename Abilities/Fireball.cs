using csharp_roguelike_rpg.Characters;
using csharp_roguelike_rpg.Systems;
namespace csharp_roguelike_rpg.Abilities;

public class Fireball : Ability
{
    public Fireball()
    {
        Name = "Fireball";
        Description = "These are small rooms, what's the worst that could happen?. Hit the target with a huge fireball for 200% of Intellect as damage";
        EnergyCost = 5;
    }

    public override void Use(Character caster, Character target)
    {
        // Fireball deals 200% of the caster's Intellect as damage
        int damage = caster.Stats["Intellect"] * 2;
        target.Health -= damage;
        UIManager.SlowPrint($"{caster.Name} hurls a ball of fire at {target.Name}, dealing {damage} damage!", ConsoleColor.Red);
    }
}