using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    // 引用
    public KitchenSystem kitchen;
    public TMP_Text resultText;
    public GameObject ingredientButtonPrefab; // 建议也改名为 itemButtonPrefab
    public Transform buttonContainer;

    // 当前玩家已选的物品 (改成 ItemData)
    private List<ItemData> selectedItems = new List<ItemData>();

    // 所有可用的物品 (改成 ItemData)
    public List<ItemData> allItems;

    void Start()
    {
        // 如果 allItems 为空，报个错提醒自己
        if (allItems == null || allItems.Count == 0)
        {
            Debug.LogError("请先在 Inspector 里给 All Items 列表添加物品！");
            return;
        }

        // 如果按钮预制体没拖，报错提醒
        if (ingredientButtonPrefab == null)
        {
            Debug.LogError("请先拖入 Item Button Prefab！");
            return;
        }

        // 如果按钮容器没拖，报错提醒
        if (buttonContainer == null)
        {
            Debug.LogError("请先拖入 Button Container！");
            return;
        }

        // 为每个物品动态生成按钮
        foreach (ItemData item in allItems)
        {
            GameObject btnObj = Instantiate(ingredientButtonPrefab, buttonContainer);
            Button btn = btnObj.GetComponent<Button>();

            TMP_Text btnText = btnObj.GetComponentInChildren<TMP_Text>();

            if (btnText != null)
            {
                btnText.text = item.itemName; // 使用 itemName
            }
            else
            {
                Debug.LogWarning("按钮预制体里找不到 TMP_Text 组件，请检查预制体");
            }

            // 绑定点击事件
            btn.onClick.AddListener(() => AddItem(item));
        }
    }

    // 添加物品到选中列表 (修复了参数类型和变量名)
    public void AddItem(ItemData item)
    {
        if (!selectedItems.Contains(item))
        {
            selectedItems.Add(item);
            Debug.Log("已选：" + item.itemName);
            UpdateSelectedDisplay();
        }
        else
        {
            Debug.Log(item.itemName + " 已经选过了");
        }
    }

    // 清空已选物品
    public void ClearSelection()
    {
        selectedItems.Clear();
        Debug.Log("已清空选择");
        UpdateSelectedDisplay();
    }

    // 调用厨房系统匹配菜谱
    public void OnCookButtonClick()
    {
        if (selectedItems.Count == 0)
        {
            resultText.text = "请先选择物品！";
            return;
        }

        // 注意：这里 MatchRecipe 方法的参数类型可能也需要改成 List<ItemData>
        RecipeData matchedRecipe = kitchen.MatchRecipe(selectedItems);

        if (matchedRecipe != null)
        {
            // 检查库存是否足够
            if (GameDataManager.Instance.HasIngredients(matchedRecipe)) // 如果 GameDataManager 里也是 ItemData，这里可能也要改
            {
                // 消耗食材
                GameDataManager.Instance.ConsumeIngredients(matchedRecipe);
                // 加钱
                GameDataManager.Instance.AddMoney(matchedRecipe.basePrice);
                // 显示结果
                resultText.text = "做出：" + matchedRecipe.recipeName + "！\n赚了 " + matchedRecipe.basePrice + " 金币";

                // 清空选择，准备下一道菜
                ClearSelection();
            }
            else
            {
                resultText.text = "食材不够！需要：";
                foreach (ItemData ing in matchedRecipe.item) // 这里也要改成 ItemData
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

    // 显示已选物品
    private void UpdateSelectedDisplay()
    {
        if (selectedItems.Count == 0)
        {
            return;
        }

        string names = "";
        foreach (ItemData item in selectedItems)
        {
            names += item.itemName + " ";
        }
        Debug.Log("当前已选：" + names);
    }
}