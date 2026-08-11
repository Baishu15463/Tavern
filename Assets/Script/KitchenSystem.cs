using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenSystem : MonoBehaviour
{
    public List<RecipeData> allRecipes;//ËùÓÐµÄ²ËÆ×

    public RecipeData MatchRecipe(List<IngredientData> selectIngredients)
    {
        if (selectIngredients == null || selectIngredients.Count == 0)
            return null;
        foreach(RecipeData recipe in allRecipes)
        {
            if(recipe.ingredients.Count != selectIngredients.Count)
                continue;
            bool allMatch = true;
            foreach(IngredientData required in recipe.ingredients)
            {
                if(!selectIngredients.Contains(required))
                {
                    allMatch = false;
                    break;
                }
            }
            if(allMatch)
                return recipe;
        }

        return null;
    }
}
