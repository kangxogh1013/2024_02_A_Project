
public class RecipeList
{
    public static CraftingRecipe[] KitchenRecipes = new CraftingRecipe[]
    {
        new CraftingRecipe
        {
            itemName = "야채 스튜",
            resultItem = ItemType.VegetableStew,
            resultAmount = 1,
            hungerRestoreAmount = 40f,
            requiredItems = new ItemType[] {ItemType.Plant , ItemType.Bush},
            requiredAmounts = new int[] {2 ,1}
        },
        new CraftingRecipe
        {
            itemName = "과일 샐러드",
            resultItem = ItemType.FruitSalad,
            resultAmount = 1,
            hungerRestoreAmount = 60f,
            requiredItems = new ItemType[] {ItemType.Plant , ItemType.Bush},
            requiredAmounts = new int[] {3 ,3}
        },
    };

    public static CraftingRecipe[] WorkbenchRecipes = new CraftingRecipe[]
    {
        new CraftingRecipe
        {
            itemName = "수리 키트",
            resultItem = ItemType.RepairKit,
            resultAmount = 1,
            repairAmount = 25f,
            requiredItems = new ItemType[] {ItemType.Crystal},
            requiredAmounts = new int[] {3}
        },
    };
}
