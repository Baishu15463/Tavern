using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    public KitchenSystem kitchen;
    public TMP_Text resultText;
    public GameObject ingredientButtonPrefab;
    public Transform buttonContainer;
    public List<ItemData> allItems;

    // ★ 直接把获得物品的大喇叭拖进来！
    public OnItemChangeSO onItemGained;

    private List<ItemData> selectedItems = new List<ItemData>();

    void Start()
    {
        // ... 生成按钮的代码保持不变 ...
    }

    public void AddItem(ItemData item)
    {
        if (!selectedItems.Contains(item)) selectedItems.Add(item);
    }

    public void ClearSelection()
    {
        selectedItems.Clear();
    }

    public void OnCookButtonClick()
    {
        if (selectedItems.Count == 0)
        {
            resultText.text = "请先选择物品！";
            return;
        }

        RecipeData matchedRecipe = kitchen.MatchRecipe(selectedItems);

        if (matchedRecipe != null)
        {
            // 1. 先检查 GameDataManager 里食材够不够
            if (GameDataManager.Instance.HasIngredients(matchedRecipe))
            {
                // 2. 够的话，扣食材、加钱
                GameDataManager.Instance.ConsumeIngredients(matchedRecipe);
                GameDataManager.Instance.AddMoney(matchedRecipe.basePrice);

                // 3. ★ 最爽的一步：直接用大喇叭把成品“喊”进背包！
                onItemGained.RaiseEvent(matchedRecipe.resultDish);

                // 4. 更新文字，清空选择
                resultText.text = "做出：" + matchedRecipe.recipeName + "！\n赚了 " + matchedRecipe.basePrice + " 金币";
                ClearSelection();
            }
            else
            {
                // 食材不够的逻辑
                resultText.text = "食材不够！需要：";
                foreach (ItemData ing in matchedRecipe.item)
                {
                    resultText.text += "\n" + ing.itemName + " x1";
                }
            }
        }
        else
        {
            resultText.text = "这些食材组合不出任何菜谱...";
        }
    }
}