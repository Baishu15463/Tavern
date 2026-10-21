using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Tavern/New Recipe")]
public class RecipeData : ScriptableObject
{
    public string recipeName;       // 菜名
    public int basePrice;           // 基础售价
    public List<ItemData> item; // 需要的食材列表
    public Sprite icon;             // 菜品图标
}