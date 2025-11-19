using csharp_roguelike_rpg.Characters;
using csharp_roguelike_rpg.Systems;

namespace csharp_roguelike_rpg.Abilities;

public class Meditate : Ability
{
    public Meditate()
    {
        Name = "Meditate";
        Description = "Center your mind. Restore 20 Energy and enter a defensive stance.";
        EnergyCost = 0;
    }

    public override void Use(Character caster, Character target)
    {
        int energyGain = 20;
        caster.Energy += energyGain;
        
        // Over energize prevention
        if (caster.Energy > caster.MaxEnergy)
        {
            caster.Energy = caster.MaxEnergy;
        }

        caster.IsDefending = true;

        UIManager.SlowPrint($"{caster.Name} takes a deep breath, restoring energy and preparing for impact.", ConsoleColor.Cyan);
    }
}