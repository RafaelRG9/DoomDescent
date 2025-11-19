using csharp_roguelike_rpg.Characters;
using csharp_roguelike_rpg.Systems;

namespace csharp_roguelike_rpg.Abilities;

public class ShieldBash : Ability
{
    public ShieldBash()
    {
        Name = "Shield Bash";
        Description = "Strike and defend all in one. Deals 100% Strength damage and activate defensive stance";
        EnergyCost = 5;
    }

    public override void Use(Character caster, Character target)
    {
        int damage = caster.GetTotalStrength();

        target.Health -= damage;

        // Turn on defense stance
        caster.IsDefending = true;

        UIManager.SlowPrint($"{caster.Name} slams their shield into {target.Name} for {damage} damage!", ConsoleColor.White);
        UIManager.SlowPrint($"{caster.Name} raises their guard against the next attack.", ConsoleColor.Gray);
    }
}