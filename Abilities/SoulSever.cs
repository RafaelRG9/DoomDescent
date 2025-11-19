using csharp_roguelike_rpg.Characters;
using csharp_roguelike_rpg.Systems;

namespace csharp_roguelike_rpg.Abilities;

public class SoulSever : Ability
{
    public SoulSever()
    {
        Name = "Soul Sever";
        Description = "Tears the soul apart. Halves the target's current health.";
        EnergyCost = 15;
    }

    public override void Use(Character caster, Character target)
    {
        int damage = target.Health / 2;
        
        if (damage < 1) damage = 1; // Always deal at least 1

        target.Health -= damage;

        UIManager.SlowPrint($"{caster.Name} severs the target's soul lifeline!", ConsoleColor.DarkMagenta);
        UIManager.SlowPrint($"The target loses half their life ({damage} damage)!", ConsoleColor.Magenta);
    }
}