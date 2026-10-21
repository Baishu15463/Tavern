using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenSystem : MonoBehaviour
{
    public List<RecipeData> allRecipes; // 所有的菜谱
    public RecipeData MatchRecipe(List<ItemData> selectedItems)
    {
        if (selectedItems == null || selectedItems.Count == 0)
            return null;

        foreach (RecipeData recipe in allRecipes)
        {
            if (recipe.item.Count != selectedItems.Count)
                continue;

            bool allMatch = true;

            foreach (ItemData required in recipe.item)
            {
                if (!selectedItems.Contains(required))
                {
                    allMatch = false;
                    break;
                }
            }

            if (allMatch)
                return recipe;
        }

        return null;
    }
}