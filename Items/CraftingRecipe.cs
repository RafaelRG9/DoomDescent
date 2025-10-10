namespace csharp_roguelike_rpg.Items;
public class CraftingRecipe
{
    // Key is the item, the value is the required quantity
    public Dictionary<Item, int> RequiredMaterials { get; private set; }
    public Item ResultingItem { get; private set; }

    public CraftingRecipe(Item result, Dictionary<Item, int> materials)
    {
        ResultingItem = result;
        RequiredMaterials = materials;
    }
}