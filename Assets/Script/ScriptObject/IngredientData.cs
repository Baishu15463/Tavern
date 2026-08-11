using UnityEngine;

[CreateAssetMenu(fileName = "NewIngredient", menuName = "Tavern/Ingredient")]
public class IngredientData : ScriptableObject
{
    public string ingredientName;   // 食材名称
    public int buyPrice;            // 购买价格（留给以后扩展）
    public Sprite icon;             // 食材图标（以后用来显示）
}