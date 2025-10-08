public class DoubleStrike : Ability
{
    public DoubleStrike()
    {
        Name = "Double Strike";
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