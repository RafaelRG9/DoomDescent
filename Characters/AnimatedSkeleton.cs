using csharp_roguelike_rpg.Items;

namespace csharp_roguelike_rpg.Characters;

public class AnimatedSkeleton : Monster
{
    public AnimatedSkeleton(Item? loot)
        : base("Animated Skeleton", 15, 6, 12, 1, 40, loot)
    {

    }
}