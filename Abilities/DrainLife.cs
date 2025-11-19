using csharp_roguelike_rpg.Characters;
using csharp_roguelike_rpg.Systems;

namespace csharp_roguelike_rpg.Abilities;

public class DrainLife : Ability
{
    public DrainLife()
    {
        Name = "Drain Life";
        Description = "You are now a vampire! suck the life force from your foes. Deals damage based on Intellect and heals you for the amount dealt.";
        EnergyCost = 12;
    }

    public override void Use(Character caster, Character target)
    {
        int damage = caster.Stats["Intellect"];

        target.Health -= damage;
        caster.Health += damage;

        // Overheal protection
        if (caster.Health > caster.MaxHealth) caster.Health = caster.MaxHealth;

        UIManager.SlowPrint($"{caster.Name} drains {damage} health from {target.Name}!", ConsoleColor.DarkMagenta);
    }
}