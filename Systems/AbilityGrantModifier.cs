using csharp_roguelike_rpg.Characters;

public class AbilityGrantModifier : Modifier
{
    private readonly Ability _abilityToGrant;

    public AbilityGrantModifier(string name, string description, Rarity rarity, Ability abilityToGrant)
    {
        Name = name;
        Description = description;
        Rarity = rarity;
        _abilityToGrant = abilityToGrant;
    }

    public override void Apply(Player player)
    {
        player.Abilities.Add(_abilityToGrant);
        UIManager.SlowPrint($"You have learned a new ability: {_abilityToGrant.Name}!", ConsoleColor.Magenta);
         UIManager.SlowPrint($"{_abilityToGrant.Description}", ConsoleColor.Magenta);
    }
}