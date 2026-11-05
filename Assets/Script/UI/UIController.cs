using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    [Header("厨房与UI引用")]
    public KitchenSystem kitchen;
    public TMP_Text resultText;
    public GameObject ingredientButtonPrefab;
    public Transform buttonContainer;

    [Header("数据与事件")]
    public List<ItemData> allItems;
    public OnItemChangeSO onItemGained; // 核心：获得物品的大喇叭

    // 玩家当前已选的物品
    private List<ItemData> selectedItems = new List<ItemData>();

    void Start()
    {
        // 1. 安全检查
        if (allItems == null || allItems.Count == 0)
        {
            Debug.LogError("【UIController】All Items 列表为空，请把食材拖进去！");
            return;
        }
        if (ingredientButtonPrefab == null || buttonContainer == null)
        {
            Debug.LogError("【UIController】按钮预制体或按钮容器没拖入！");
            return;
        }

        // 2. 生成按钮
        foreach (ItemData item in allItems)
        {
            GameObject btnObj = Instantiate(ingredientButtonPrefab, buttonContainer);
            Button btn = btnObj.GetComponent<Button>();
            TMP_Text btnText = btnObj.GetComponentInChildren<TMP_Text>();

            if (btnText != null)
            {
                btnText.text = item.itemName;
            }
            else
            {
                Debug.LogWarning("【UIController】按钮预制体里找不到 TMP_Text 组件！");
            }

            // 绑定点击事件：点击按钮时，调用 AddItem 方法
            btn.onClick.AddListener(() => AddItem(item));
        }
    }

    // 把物品加入已选列表
    public void AddItem(ItemData item)
    {
        if (!selectedItems.Contains(item))
        {
            selectedItems.Add(item);
            Debug.Log("已选：" + item.itemName);
            // 如果你有显示“当前已选什么”的UI，可以在这里更新
        }
        else
        {
            Debug.Log(item.itemName + " 已经选过了");
        }
    }

    // 清空已选列表
    public void ClearSelection()
    {
        selectedItems.Clear();
        Debug.Log("已清空选择");
    }

    // 点击“烹饪”按钮时触发
    public void OnCookButtonClick()
    {
        // 1. 检查是否选了食材
        if (selectedItems.Count == 0)
        {
            resultText.text = "请先选择物品！";
            return;
        }

        // 2. 匹配菜谱
        RecipeData matchedRecipe = kitchen.MatchRecipe(selectedItems);

        if (matchedRecipe != null)
        {
            // 3. 检查 GameDataManager 里食材够不够
            if (GameDataManager.Instance.HasIngredients(matchedRecipe))
            {
                // 4. 扣食材、加钱
                GameDataManager.Instance.ConsumeIngredients(matchedRecipe);
                GameDataManager.Instance.AddMoney(matchedRecipe.basePrice);

                // 5. ★ 用大喇叭把成品“喊”进背包（触发事件）
                if (onItemGained != null && matchedRecipe.resultDish != null)
                {
                    onItemGained.RaiseEvent(matchedRecipe.resultDish);
                }
                else
                {
                    Debug.LogError("【UIController】做菜成功，但大喇叭没挂载，或者菜谱没设置成品！");
                }

                // 6. 更新文字提示，清空选择
                resultText.text = "做出：" + matchedRecipe.recipeName + "！\n赚了 " + matchedRecipe.basePrice + " 金币";
                ClearSelection();
            }
            else
            {
                // 7. 食材不够的逻辑
                resultText.text = "食材不够！需要：";
                foreach (ItemData ing in matchedRecipe.item)
                {
                    resultText.text += "\n" + ing.itemName + " x1";
                }
            }
        }
        else
        {
            // 8. 没有匹配到任何菜谱
            resultText.text = "这些食材组合不出任何菜谱...";
        }
    }
}